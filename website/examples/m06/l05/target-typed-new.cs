Alert first = new();
Alert second = new(3);
Alert third = new(2) { Title = "Many failed logins" };

List<Alert> alerts = new();
alerts.Add(first);
alerts.Add(new(1) { Title = "Test" });

Console.WriteLine(first.Level);
Console.WriteLine(second.Level);
Console.WriteLine(third.Level);
Console.WriteLine(alerts.Count);

class Alert
{
    private int level = 1;
    private string title = "";

    public Alert()
    {
    }

    public Alert(int level)
    {
        this.level = level;
    }

    public int Level
    {
        get { return level; }
    }

    public string Title
    {
        get { return title; }
        set { title = value; }
    }
}
