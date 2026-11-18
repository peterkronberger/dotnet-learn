using Microsoft.Extensions.Configuration;

public sealed class AppOptions
{
    public string Endpoint { get; set; } = "";

    [ConfigurationIgnore]
    public string RuntimeValue { get; set; } = "calculated";
}
