Sensor sensor = new Sensor();
sensor.Reading = 5;
int first = sensor.Reading;
int second = sensor.Reading;
Console.WriteLine(first + second);

class Sensor
{
    private int reading;

    public int Reading
    {
        get
        {
            Console.WriteLine("get runs");
            return reading;
        }
        set
        {
            Console.WriteLine($"set runs with {value}");
            reading = value;
        }
    }
}
