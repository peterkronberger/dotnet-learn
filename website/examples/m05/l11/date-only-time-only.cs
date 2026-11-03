using System.Globalization;

DateOnly day = new DateOnly(2026, 3, 2);
TimeOnly time = new TimeOnly(8, 1, 40);

Console.WriteLine(day.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
Console.WriteLine(day.AddDays(30).ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
Console.WriteLine(time.ToString("HH:mm:ss", CultureInfo.InvariantCulture));
