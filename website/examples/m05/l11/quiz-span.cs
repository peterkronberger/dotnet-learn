using System.Globalization;

DateTimeOffset a = DateTimeOffset.Parse("2026-03-02T09:10:09Z", CultureInfo.InvariantCulture);
DateTimeOffset b = DateTimeOffset.Parse("2026-03-02T09:10:41Z", CultureInfo.InvariantCulture);

TimeSpan gap = b - a;
Console.WriteLine(gap);
Console.WriteLine(gap.TotalSeconds);
Console.WriteLine(gap < TimeSpan.FromMinutes(1));
