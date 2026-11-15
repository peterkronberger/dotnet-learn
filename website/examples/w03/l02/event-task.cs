Sensor sensor = new();
sensor.Alarm += message => Console.WriteLine($"alarm: {message}");
sensor.Check(95);
sensor.Check(40);

class Sensor
{
    // TODO: make Alarm an event, so that outside code can only subscribe and unsubscribe
    public Action<string>? Alarm;

    public void Check(int degrees)
    {
        if (degrees > 90)
        {
            Alarm?.Invoke($"{degrees} degrees");
        }
    }
}
