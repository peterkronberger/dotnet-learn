using System.Diagnostics;
using System.Reflection;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace Playground;

/// <summary>Eine Zeile der Compiler-Ausgabe.</summary>
public sealed record DiagnosticLine(bool IsError, string Text);

/// <summary>Ergebnis eines Durchlaufs aus Kompilieren und Ausführen.</summary>
public sealed record RunResult(
    bool Success,
    string Output,
    IReadOnlyList<DiagnosticLine> Diagnostics,
    TimeSpan Duration)
{
    public string Summary => Success
        ? $"Fertig in {Duration.TotalMilliseconds:0} ms"
        : "Kompilierung fehlgeschlagen";
}

/// <summary>
/// Kompiliert C#-Code mit Roslyn im Browser und führt ihn aus.
/// Alles läuft lokal in der WebAssembly-Sandbox, es wird nichts an einen Server gesendet.
/// </summary>
public sealed class CSharpRunner
{
    // Entspricht den „Implicit Usings“ eines Konsolenprojekts.
    private const string GlobalUsings =
        "global using System;\n" +
        "global using System.Collections.Generic;\n" +
        "global using System.IO;\n" +
        "global using System.Linq;\n" +
        "global using System.Threading;\n" +
        "global using System.Threading.Tasks;\n";

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

    public async Task<RunResult> RunAsync(string code)
    {
        var started = Stopwatch.GetTimestamp();

        var parseOptions = new CSharpParseOptions(LanguageVersion.Latest);
        var usingsTree = CSharpSyntaxTree.ParseText(GlobalUsings, parseOptions, path: "GlobalUsings.cs");
        var userTree = CSharpSyntaxTree.ParseText(code, parseOptions, path: "Program.cs");

        var compilation = CSharpCompilation.Create(
            assemblyName: "UserProgram",
            syntaxTrees: new[] { usingsTree, userTree },
            references: References.Value,
            options: new CSharpCompilationOptions(
                OutputKind.ConsoleApplication,
                optimizationLevel: OptimizationLevel.Release,
                nullableContextOptions: NullableContextOptions.Enable,
                // WebAssembly läuft hier einthreadig.
                concurrentBuild: false));

        using var peStream = new MemoryStream();
        var emit = compilation.Emit(peStream);

        var diagnostics = emit.Diagnostics
            .Where(d => d.Severity is DiagnosticSeverity.Error or DiagnosticSeverity.Warning)
            .Select(FormatDiagnostic)
            .ToList();

        if (!emit.Success)
        {
            return new RunResult(false, string.Empty, diagnostics, Stopwatch.GetElapsedTime(started));
        }

        var output = new StringWriter();
        var originalOut = Console.Out;
        var originalError = Console.Error;
        Console.SetOut(output);
        Console.SetError(output);

        try
        {
            var loaded = Assembly.Load(peStream.ToArray());
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
        catch (TargetInvocationException ex)
        {
            var inner = ex.InnerException ?? ex;
            output.WriteLine($"Unbehandelte Ausnahme: {inner.GetType().Name}: {inner.Message}");
        }
        catch (Exception ex)
        {
            output.WriteLine($"Fehler: {ex.GetType().Name}: {ex.Message}");
        }
        finally
        {
            Console.SetOut(originalOut);
            Console.SetError(originalError);
        }

        return new RunResult(true, output.ToString(), diagnostics, Stopwatch.GetElapsedTime(started));
    }

    /// <summary>
    /// Für asynchrone Einstiegspunkte (auch "await" in Top-Level-Anweisungen) erzeugt der Compiler
    /// einen synchronen Wrapper, der den Task blockierend abwartet. Das scheitert in der
    /// einthreadigen WebAssembly-Laufzeit ("Cannot wait on monitors on this runtime").
    /// Deshalb wird die asynchrone Methode direkt aufgerufen und von <see cref="RunAsync"/> awaited.
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

        if (diagnostic.Location.IsInSource)
        {
            var position = diagnostic.Location.GetLineSpan().StartLinePosition;
            return new DiagnosticLine(isError,
                $"{kind} {diagnostic.Id} (Zeile {position.Line + 1}, Spalte {position.Character + 1}): {message}");
        }

        return new DiagnosticLine(isError, $"{kind} {diagnostic.Id}: {message}");
    }
}
