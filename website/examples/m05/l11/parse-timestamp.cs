using System.Globalization;

string text = "2026-03-02T08:01:40Z";
DateTimeOffset moment = DateTimeOffset.Parse(text, CultureInfo.InvariantCulture);

Console.WriteLine(moment.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
Console.WriteLine(moment.Offset);
Console.WriteLine(moment.UtcDateTime.Hour);
