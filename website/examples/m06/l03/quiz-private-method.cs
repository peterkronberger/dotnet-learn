Meter meter = new Meter();
meter.Record(4);
meter.Record(-1);
meter.Record(2);
Console.WriteLine(meter.Total());

class Meter
{
    private int total;

    private bool IsValid(int number)
    {
        return number > 0;
    }

    public void Record(int number)
    {
        if (IsValid(number))
        {
            total += number;
        }
    }

    public int Total()
    {
        return total;
    }
}
