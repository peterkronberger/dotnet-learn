Reading reading = new(25);
Console.WriteLine(reading.Fahrenheit);
Console.WriteLine(reading.IsWarm());
Console.WriteLine(Reading.Unit);

class Reading(double celsius)
{
    public double Celsius => celsius;
}

static class ReadingExtensions
{
    extension(Reading reading)
    {
        public double Fahrenheit => reading.Celsius * 9 / 5 + 32;

        public bool IsWarm()
        {
            return reading.Celsius > 20;
        }
    }

    extension(Reading)
    {
        public static string Unit => "Celsius";
    }
}
