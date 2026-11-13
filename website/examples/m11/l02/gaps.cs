// Task 1: rewrite IsHot as a member of an extension block
// Task 2: add the extension property Kelvin (Celsius + 273.15)
// Task 3: print both, the calls look the same as before
Reading reading = new(25);
Console.WriteLine(reading.IsHot());

class Reading(double celsius)
{
    public double Celsius => celsius;
}

static class ReadingExtensions
{
    public static bool IsHot(this Reading reading)
    {
        return reading.Celsius > 30;
    }
}
