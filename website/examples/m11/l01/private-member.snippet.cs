class Sensor
{
    private int secret = 42;
}

static class SensorExtensions
{
    public static int Peek(this Sensor sensor)
    {
        return sensor.secret;
    }
}
