using System.Diagnostics;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Playground;

/// <summary>One line of compiler output.</summary>
public sealed record DiagnosticLine(bool IsError, string Text, int? Line = null);

/// <summary>Outcome of one task test. For hidden tests (<c>Visible = false</c>) the name is neutral.</summary>
public sealed record TestOutcome(string Name, bool Visible, bool Passed, string Feedback);

/// <summary>Result of one compile-and-run pass.</summary>
public sealed record RunResult(
    bool Success,
    string Output,
    IReadOnlyList<DiagnosticLine> Diagnostics,
    TimeSpan Duration,
    IReadOnlyList<TestOutcome>? Tests = null,
    StepTrace? Trace = null,
    bool TraceUnavailable = false)
{
    public string Summary => Success
        ? $"Fertig in {Duration.TotalMilliseconds:0} ms"
        : "Kompilierung fehlgeschlagen";
}

/// <summary>
/// Compiles C# code with Roslyn in the browser and runs it.
/// Everything runs locally in the WebAssembly sandbox; nothing is sent to a server.
/// </summary>
public sealed class CSharpRunner
{
    // Matches the "implicit usings" of a console project.
    private const string GlobalUsings =
        "global using System;\n" +
        "global using System.Collections.Generic;\n" +
        "global using System.IO;\n" +
        "global using System.Linq;\n" +
        "global using System.Threading;\n" +
        "global using System.Threading.Tasks;\n";

    private const int TimeoutSeconds = 3;
    private const string TestsPath = "Tests.cs";
    private const string TestsTypeName = "TaskTests";

    private static readonly Lazy<IReadOnlyList<MetadataReference>> References = new(LoadReferences);

    private static IReadOnlyList<MetadataReference> LoadReferences()
    {
        var assembly = typeof(CSharpRunner).Assembly;
        var references = new List<MetadataReference>();

        foreach (var name in assembly.GetManifestResourceNames())
        {
            if (!name.StartsWith("refs/", StringComparison.Ordinal))
            {
                continue;
            }

            using var stream = assembly.GetManifestResourceStream(name)!;
            using var buffer = new MemoryStream();
            stream.CopyTo(buffer);
            references.Add(MetadataReference.CreateFromImage(buffer.ToArray()));
        }

        return references;
    }

    /// <param name="code">Learner code.</param>
    /// <param name="testsCode">
    /// Optional test code (spike S7). It declares <c>public static class TaskTests</c> with
    /// <c>public static (string Name, bool Visible, bool Passed, string Feedback)[] Run()</c>.
    /// If given, the tests run instead of the program's entry point.
    /// </param>
    /// <param name="trace">
    /// Record every step (spike S5). Ignored when tests run. If the instrumented code does not
    /// compile, the program runs normally and <see cref="RunResult.TraceUnavailable"/> is set.
    /// </param>
    /// <param name="stdin">
    /// Pre-filled console input. Each line is read by one <c>Console.ReadLine()</c>; after the last line it
    /// returns <c>null</c>. Without input, the reader is empty, so reading never blocks.
    /// </param>
    public async Task<RunResult> RunAsync(string code, string? testsCode = null, bool trace = false, string? stdin = null)
    {
        var started = Stopwatch.GetTimestamp();

        var parseOptions = new CSharpParseOptions(LanguageVersion.Latest);
        var usingsTree = CSharpSyntaxTree.ParseText(GlobalUsings, parseOptions, path: "GlobalUsings.cs");
        var userTree = CSharpSyntaxTree.ParseText(code, parseOptions, path: "Program.cs");
        var testsTree = string.IsNullOrWhiteSpace(testsCode)
            ? null
            : CSharpSyntaxTree.ParseText(testsCode, parseOptions, path: TestsPath);

        // The guard is inserted for execution only. Diagnostics come from the unmodified code,
        // so line and column numbers stay exact.
        var guardTree = CSharpSyntaxTree.ParseText(LoopGuardRewriter.GuardSource, parseOptions, path: "Guard.cs");
        SyntaxTree Guarded(SyntaxTree tree) =>
            CSharpSyntaxTree.Create((CSharpSyntaxNode)LoopGuardRewriter.Rewrite(tree), parseOptions, path: "Program.cs");

        var guardedTree = Guarded(userTree);

        var guarded = Compile(WithTests([usingsTree, guardedTree, guardTree], testsTree));
        var diagnostics = new List<DiagnosticLine>();
        var image = guarded.Success ? guarded.Image : null;

        if (guarded.Diagnostics.Count > 0 || image is null)
        {
            var plain = Compile(WithTests([usingsTree, userTree], testsTree));
            diagnostics = plain.Diagnostics.Select(FormatDiagnostic).ToList();
            if (!plain.Success)
            {
                return new RunResult(false, string.Empty, diagnostics, Stopwatch.GetElapsedTime(started));
            }

            // The guarded variant failed to compile although the original did: run without guard.
            image ??= plain.Image;
        }

        // Step view: compile a second variant with the trace calls (after the guard, so the guard stays in).
        StepRecorder? recorder = null;
        var traceUnavailable = false;
        var output = new StringWriter();
        if (trace && testsTree is null && image is not null)
        {
            var tracedTree = CSharpSyntaxTree.Create(
                (CSharpSyntaxNode)StepTraceRewriter.Rewrite(userTree), parseOptions, path: "Program.cs");
            var traceSupport = CSharpSyntaxTree.ParseText(StepTraceRewriter.TraceSource, parseOptions, path: "Trace.cs");
            var traced = Compile([usingsTree, Guarded(tracedTree), guardTree, traceSupport]);
            if (traced.Success)
            {
                image = traced.Image;
                recorder = new StepRecorder(() => output.ToString());
            }
            else
            {
                traceUnavailable = true;
            }
        }

        IReadOnlyList<TestOutcome>? tests = null;
        var originalOut = Console.Out;
        var originalError = Console.Error;
        // Console.In is not supported in the browser (WebAssembly); reading it throws PlatformNotSupportedException.
        TextReader? originalIn = null;
        try { originalIn = Console.In; }
        catch (PlatformNotSupportedException) { }
        Console.SetOut(output);
        Console.SetError(output);
        Console.SetIn(new StringReader(stdin ?? string.Empty));

        try
        {
            var loaded = Assembly.Load(image!);
            loaded.GetType(LoopGuardRewriter.GuardTypeName)
                ?.GetMethod("Reset")?.Invoke(null, new object[] { Stopwatch.Frequency * TimeoutSeconds });
            if (recorder is not null)
            {
                var hook = loaded.GetType(StepTraceRewriter.TraceTypeName)?.GetField("Hook");
                hook?.SetValue(null, (Action<int, string[], object?[]>)recorder.Record);
            }

            if (testsTree is not null)
            {
                tests = RunTests(loaded);
            }
            else
            {
                var entryPoint = ResolveEntryPoint(loaded)
                    ?? throw new InvalidOperationException("Es wurde kein Einstiegspunkt gefunden.");

                object?[]? arguments = entryPoint.GetParameters().Length == 0
                    ? null
                    : new object?[] { Array.Empty<string>() };

                if (entryPoint.Invoke(null, arguments) is Task task)
                {
                    await task;
                }
            }
        }
        catch (TargetInvocationException ex)
        {
            var inner = ex.InnerException ?? ex;
            if (testsTree is not null)
            {
                tests = [new TestOutcome("Aufgabentests", true, false, $"Die Tests konnten nicht ausgeführt werden: {inner.GetType().Name}: {inner.Message}")];
            }
            else if (!TryWriteTimeout(inner, output))
            {
                output.WriteLine($"Unbehandelte Ausnahme: {inner.GetType().Name}: {inner.Message}");
            }
        }
        catch (Exception ex)
        {
            if (!TryWriteTimeout(ex, output))
            {
                output.WriteLine($"Fehler: {ex.GetType().Name}: {ex.Message}");
            }
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
            Console.SetIn(originalIn ?? TextReader.Null);
        }

        return new RunResult(true, output.ToString(), diagnostics, Stopwatch.GetElapsedTime(started), tests,
            recorder?.ToTrace(), traceUnavailable);
    }

    private static IEnumerable<SyntaxTree> WithTests(IEnumerable<SyntaxTree> trees, SyntaxTree? testsTree) =>
        testsTree is null ? trees : trees.Append(testsTree);

    private static IReadOnlyList<TestOutcome> RunTests(Assembly assembly)
    {
        var run = assembly.GetType(TestsTypeName)?.GetMethod("Run", BindingFlags.Public | BindingFlags.Static);
        if (run?.Invoke(null, null) is not (string Name, bool Visible, bool Passed, string Feedback)[] raw)
        {
            return [new TestOutcome("Aufgabentests", true, false, "Die Aufgabe enthält keine ausführbaren Tests.")];
        }

        var hidden = 0;
        return raw.Select(t => new TestOutcome(
            t.Visible ? t.Name : $"Versteckter Test {++hidden}", t.Visible, t.Passed, t.Feedback)).ToList();
    }

    private static (bool Success, byte[] Image, List<Diagnostic> Diagnostics) Compile(IEnumerable<SyntaxTree> trees)
    {
        var compilation = CSharpCompilation.Create(
            assemblyName: "UserProgram",
            syntaxTrees: trees,
            references: References.Value,
            options: new CSharpCompilationOptions(
                OutputKind.ConsoleApplication,
                optimizationLevel: OptimizationLevel.Release,
                nullableContextOptions: NullableContextOptions.Enable,
                // WebAssembly is single-threaded here.
                concurrentBuild: false));

        using var peStream = new MemoryStream();
        var emit = compilation.Emit(peStream);
        var diagnostics = emit.Diagnostics
            .Where(d => d.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning)
            .ToList();
        return (emit.Success, peStream.ToArray(), diagnostics);
    }

    private static bool TryWriteTimeout(Exception exception, StringWriter output)
    {
        if (exception.GetType().Name != LoopGuardRewriter.ExceptionTypeName)
        {
            return false;
        }

        var line = exception.GetType().GetProperty("Line")?.GetValue(exception);
        output.WriteLine($"Dein Programm läuft vermutlich endlos – prüfe die Abbruchbedingung in Zeile {line}.");
        return true;
    }

    /// <summary>
    /// For async entry points (also "await" in top-level statements) the compiler generates
    /// a synchronous wrapper that blocks on the task. This fails on the single-threaded
    /// WebAssembly runtime ("Cannot wait on monitors on this runtime").
    /// So the async method is called directly and awaited by <see cref="RunAsync"/>.
    /// </summary>
    private static MethodInfo? ResolveEntryPoint(Assembly assembly)
    {
        var entryPoint = assembly.EntryPoint;
        var type = entryPoint?.DeclaringType;

        if (entryPoint is null || type is null)
        {
            return entryPoint;
        }

        const BindingFlags flags =
            BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.DeclaredOnly;

        foreach (var name in new[] { "<Main>$", "Main" })
        {
            var asyncMethod = type.GetMethods(flags)
                .FirstOrDefault(m => m.Name == name && typeof(Task).IsAssignableFrom(m.ReturnType));

            if (asyncMethod is not null)
            {
                return asyncMethod;
            }
        }

        return entryPoint;
    }

    private static DiagnosticLine FormatDiagnostic(Diagnostic diagnostic)
    {
        var isError = diagnostic.Severity == DiagnosticSeverity.Error;
        var kind = isError ? "Fehler" : "Warnung";
        var message = diagnostic.GetMessage();

        if (diagnostic.Location.IsInSource && diagnostic.Location.SourceTree?.FilePath == TestsPath)
        {
            // No line number: the learner cannot see the test code.
            return new DiagnosticLine(isError, $"{kind} {diagnostic.Id} (Prüfcode): {message}");
        }

        if (diagnostic.Location.IsInSource)
        {
            var position = diagnostic.Location.GetLineSpan().StartLinePosition;
            return new DiagnosticLine(isError,
                $"{kind} {diagnostic.Id} (Zeile {position.Line + 1}, Spalte {position.Character + 1}): {message}",
                position.Line + 1);
        }

        return new DiagnosticLine(isError, $"{kind} {diagnostic.Id}: {message}");
    }
}
