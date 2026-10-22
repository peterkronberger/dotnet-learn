using Microsoft.AspNetCore.Components;
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

    protected override void OnInitialized()
    {
        // Die Lernseite übergibt Beispielcode als Base64url im URL-Fragment (#code=...).
        var fragment = Navigation.ToAbsoluteUri(Navigation.Uri).Fragment;

        if (fragment.StartsWith(FragmentPrefix, StringComparison.Ordinal))
        {
            var decoded = CodeCodec.TryDecode(fragment[FragmentPrefix.Length..]);

            if (!string.IsNullOrWhiteSpace(decoded))
            {
                _initialCode = decoded;
                _code = decoded;
            }
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

        // Dem Browser Zeit geben, die Statusmeldung zu zeichnen, bevor der Compiler den Hauptthread belegt.
        await Task.Delay(50);

        try
        {
            _result = await Runner.RunAsync(_code);
        }
        finally
        {
            _running = false;
        }
    }

    private void Reset()
    {
        _code = _initialCode;
        _result = null;
    }
}
