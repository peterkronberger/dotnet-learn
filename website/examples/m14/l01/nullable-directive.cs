#:property TreatWarningsAsErrors=true
string? lastIp = null;
#nullable disable
string plain = null;
Console.WriteLine($"plain is null: {plain == null}");
#nullable enable
Console.WriteLine($"lastIp is null: {lastIp is null}");
