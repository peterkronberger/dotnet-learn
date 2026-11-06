Alarm alarm = new();
Action hello = () => Console.WriteLine("hello");
alarm.Rang += hello;
alarm.Rang += () => Console.WriteLine("again");
alarm.Rang -= hello;
alarm.Rang -= () => Console.WriteLine("again");
alarm.Ring();

class Alarm
{
    public event Action? Rang;

    public void Ring()
    {
        Rang?.Invoke();
    }
}
