using Microsoft.AspNetCore.Components;

namespace Playground;

public partial class App
{
    private const string FragmentPrefix = "#code=";

    [Inject] private NavigationManager Navigation { get; set; } = default!;
    [Inject] private CSharpRunner Runner { get; set; } = default!;

    private string _initialCode = Samples.Default;
    private string _code = Samples.Default;
    private bool _running;
    private RunResult? _result;

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

    private async Task RunAsync()
    {
        if (_running)
        {
            return;
        }

        _running = true;
        _result = null;
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
