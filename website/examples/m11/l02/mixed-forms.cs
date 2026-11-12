Reading reading = new(25);
Console.WriteLine(reading.IsWarm());
Console.WriteLine(reading.IsHot());
Console.WriteLine(ReadingExtensions.IsHot(reading));

class Reading(double celsius)
{
    public double Celsius => celsius;
}

static class ReadingExtensions
{
    public static bool IsWarm(this Reading reading)
    {
        return reading.Celsius > 20;
    }

    extension(Reading reading)
    {
        public bool IsHot()
        {
            return reading.Celsius > 30;
        }
    }
}
