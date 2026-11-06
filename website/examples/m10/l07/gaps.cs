// Task 1: declare the event Raised (type Action<int>?) in Sensor
// Task 2: raise it in Measure with the value, safe if nobody subscribed
// Task 3: subscribe PrintValue before the first Measure, unsubscribe it before the second
Sensor sensor = new();
sensor.Measure(1);
sensor.Measure(2);

PrintValue(0);

static void PrintValue(int value)
{
    Console.WriteLine($"value: {value}");
}

class Sensor
{
    public void Measure(int value)
    {
        Console.WriteLine($"measured {value}");
    }
}
