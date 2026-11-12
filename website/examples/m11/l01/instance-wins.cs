Sensor sensor = new();
Console.WriteLine(sensor.Describe());
Console.WriteLine(SensorExtensions.Describe(sensor));
Console.WriteLine(sensor.Describe("lab"));

class Sensor
{
    public string Describe()
    {
        return "instance method";
    }
}

static class SensorExtensions
{
    public static string Describe(this Sensor sensor)
    {
        return "extension method";
    }

    public static string Describe(this Sensor sensor, string place)
    {
        return $"extension method, place {place}";
    }
}
