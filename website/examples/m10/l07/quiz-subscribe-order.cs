Alarm alarm = new();
alarm.Rang += () => Console.WriteLine("a");
alarm.Rang += () => Console.WriteLine("b");
alarm.Ring();
alarm.Ring();

class Alarm
{
    public event Action? Rang;

    public void Ring()
    {
        Console.WriteLine("ring");
        Rang?.Invoke();
    }
}
