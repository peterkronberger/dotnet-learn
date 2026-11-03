using System.Globalization;

DateTimeOffset first = DateTimeOffset.Parse("2026-03-02T08:01:40Z", CultureInfo.InvariantCulture);
DateTimeOffset last = DateTimeOffset.Parse("2026-03-02T08:15:03Z", CultureInfo.InvariantCulture);

TimeSpan gap = first - last;
Console.WriteLine(gap <= TimeSpan.FromMinutes(10));
