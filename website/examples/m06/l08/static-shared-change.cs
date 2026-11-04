Alarm night = new Alarm();
Alarm day = new Alarm();
Console.WriteLine(night.IsRaised(4));
Alarm.Limit = 5;
Console.WriteLine(night.IsRaised(4));
Console.WriteLine(day.IsRaised(4));

class Alarm
{
    public static int Limit = 3;

    public bool IsRaised(int failures)
    {
        return failures >= Limit;
    }
}
