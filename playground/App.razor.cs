using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;

namespace Playground;

public partial class App
{
    private const string FragmentPrefix = "#code=";

    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private CSharpRunner Runner { get; set; } = default!;
    [Inject] private IJSRuntime Js { get; set; } = default!;

    private string _initialCode = Samples.Default;
    private string _code = Samples.Default;
    private bool _running;
    private RunResult? _result;
    private string? _compiledCode;
    private string? _instanceId;
    private string? _testsCode;
    private bool _trace;
    private int _stepIndex;

    private const int RowHeight = 28;
    private const int ArrowWidth = 72;

    protected override void OnInitialized()
    {
        // The learning site passes example code as Base64url in the URL fragment (#code=...).
        // Weitere Schlüssel (spike S7): &id=<instance id>, &tests=<Base64url test code>; (spike S5): &trace=1.
        var fragment = Navigation.ToAbsoluteUri(Navigation.Uri).Fragment;

        if (fragment.StartsWith(FragmentPrefix, StringComparison.Ordinal))
        {
            var parts = fragment[FragmentPrefix.Length..].Split('&');
            var decoded = CodeCodec.TryDecode(parts[0]);

            if (!string.IsNullOrWhiteSpace(decoded))
            {
                _initialCode = decoded;
                _code = decoded;
            }

            foreach (var part in parts.Skip(1))
            {
                if (part.StartsWith("id=", StringComparison.Ordinal))
                {
                    _instanceId = Uri.UnescapeDataString(part[3..]);
                }
                else if (part == "trace=1")
                {
                    _trace = true;
                }
                else if (part.StartsWith("tests=", StringComparison.Ordinal))
                {
                    _testsCode = CodeCodec.TryDecode(part[6..]);
                }
            }
        }
    }

    // Reports the result to the embedding page (spike S7). The origin check
    // (window.location.origin) happens in index.html (pgBridge.report).
    private async Task ReportAsync(RunResult result)
    {
        var message = new
        {
            type = "run-result",
            id = _instanceId,
            success = result.Success,
            output = result.Output,
            diagnostics = result.Diagnostics.Select(d => new { isError = d.IsError, text = d.Text, line = d.Line }).ToArray(),
            tests = result.Tests?.Select(t => new { name = t.Name, visible = t.Visible, passed = t.Passed, feedback = t.Feedback }).ToArray(),
        };

        try
        {
            await Js.InvokeVoidAsync("pgBridge.report", message);
        }
        catch (JSException)
        {
            // Standalone use: no embedding page to report to.
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        // Syntax colors and line marks live in index.html (pgEditor); they are re-applied after every render.
        // Line numbers refer to the compiled code; once the user edits it, the marks are dropped.
        var diagnostics = _result is not null && _code == _compiledCode ? _result.Diagnostics : [];
        var errorLines = diagnostics.Where(d => d.IsError && d.Line is not null).Select(d => d.Line!.Value).Distinct().ToArray();
        var warningLines = diagnostics.Where(d => !d.IsError && d.Line is not null).Select(d => d.Line!.Value).Distinct().ToArray();

        try
        {
            await Js.InvokeVoidAsync("pgEditor.refresh", errorLines, warningLines);
        }
        catch (JSException)
        {
            // The editor still works as a plain textarea without the overlay.
        }
    }

    private async Task RunAsync()
    {
        if (_running)
        {
            return;
        }

        _running = true;
        _result = null;
        _compiledCode = _code;
        StateHasChanged();

        // Give the browser time to paint the status message before the compiler occupies the main thread.
        await Task.Delay(50);

        try
        {
            _result = await Runner.RunAsync(_code, _testsCode, _trace);
            _stepIndex = 0;
        }
        finally
        {
            _running = false;
        }

        await ReportAsync(_result);
    }

    private void Reset()
    {
        _code = _initialCode;
        _result = null;
    }

    private StepTrace? Trace => _result?.Trace is { Steps.Count: > 0 } trace ? trace : null;

    private StepInfo CurrentStep => Trace!.Steps[_stepIndex];

    private string[] CodeLines => (_compiledCode ?? string.Empty).Replace("\r\n", "\n").Split('\n');

    private void GoToStep(int index) => _stepIndex = Math.Clamp(index, 0, Trace!.Steps.Count - 1);

    private void OnStepKey(KeyboardEventArgs e)
    {
        switch (e.Key)
        {
            case "ArrowRight": GoToStep(_stepIndex + 1); break;
            case "ArrowLeft": GoToStep(_stepIndex - 1); break;
            case "Home": GoToStep(0); break;
            case "End": GoToStep(Trace!.Steps.Count - 1); break;
        }
    }

    private static int ArrowY(int row) => row * RowHeight + RowHeight / 2;
}
