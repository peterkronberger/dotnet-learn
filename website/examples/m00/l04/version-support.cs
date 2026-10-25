// Estimate the end of support from a release date: LTS + 3 years, STS + 24 months.
var release = new DateOnly(2025, 11, 11);
var ltsEstimate = release.AddYears(3);
var stsEstimate = release.AddMonths(24);
var official = new DateOnly(2028, 11, 14);

Console.WriteLine($"Release: {release:yyyy-MM-dd}");
Console.WriteLine($"LTS estimate (+3 years): {ltsEstimate:yyyy-MM-dd}");
Console.WriteLine($"STS estimate (+24 months): {stsEstimate:yyyy-MM-dd}");
Console.WriteLine($"Official end for .NET 10: {official:yyyy-MM-dd}");
Console.WriteLine($"Difference in days: {official.DayNumber - ltsEstimate.DayNumber}");
