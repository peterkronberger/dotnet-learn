Alarm bobAlarm = new Alarm("bob");
Alarm daveAlarm = new Alarm("dave");

for (int attempt = 1; attempt <= 5; attempt++)
{
    bobAlarm.AddFailure();
}
daveAlarm.AddFailure();

Console.WriteLine(bobAlarm.Describe());
Console.WriteLine(daveAlarm.Describe());

class Alarm
{
    public const int Limit = 5;
    public readonly string User;
    public int Failures;

    public Alarm(string user)
    {
        User = user;
    }

    public void AddFailure()
    {
        Failures++;
    }

    public string Describe()
    {
        bool triggered = Failures >= Limit;
        return $"{User}: {Failures} of {Limit}, alarm: {triggered}";
    }
}
