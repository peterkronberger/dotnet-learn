Reading reading = new() { Celsius = 10 };
Console.WriteLine(reading.Fahrenheit);
reading.Celsius = 30;
Console.WriteLine(reading.Fahrenheit);

class Reading
{
    public double Celsius { get; set; }
}

static class ReadingExtensions
{
    extension(Reading reading)
    {
        public double Fahrenheit => reading.Celsius * 9 / 5 + 32;
    }
}
