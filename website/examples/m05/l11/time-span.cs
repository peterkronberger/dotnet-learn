using System.Globalization;

DateTimeOffset first = DateTimeOffset.Parse("2026-03-02T08:01:40Z", CultureInfo.InvariantCulture);
DateTimeOffset last = DateTimeOffset.Parse("2026-03-02T08:06:48Z", CultureInfo.InvariantCulture);

TimeSpan gap = last - first;
Console.WriteLine(gap);
Console.WriteLine(gap.TotalMinutes > 5);

TimeSpan limit = TimeSpan.FromMinutes(10);
Console.WriteLine(gap <= limit);
