Reading reading = new(25);
Console.WriteLine(reading.GetFahrenheit());
Console.WriteLine(reading.IsWarm());

class Reading(double celsius)
{
    public double Celsius => celsius;
}

static class ReadingExtensions
{
    public static double GetFahrenheit(this Reading reading)
    {
        return reading.Celsius * 9 / 5 + 32;
    }

    public static bool IsWarm(this Reading reading)
    {
        return reading.Celsius > 20;
    }
}
