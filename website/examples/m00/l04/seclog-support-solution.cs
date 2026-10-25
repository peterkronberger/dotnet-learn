// SecLog runs on .NET 8. The support policy page lists 2026-11-10 as its end of support.
// Teaching simplification: the end date itself already counts as "not supported".
var endOfSupport = new DateOnly(2026, 11, 10);
var checkDate = new DateOnly(2026, 12, 1);
var supported = checkDate < endOfSupport;

Console.WriteLine("Runtime: .NET 8");
Console.WriteLine($"End of support: {endOfSupport:yyyy-MM-dd}");
Console.WriteLine($"Check date: {checkDate:yyyy-MM-dd}");
Console.WriteLine($"Supported: {supported}");
