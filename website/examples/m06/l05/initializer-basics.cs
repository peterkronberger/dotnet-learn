Alert first = new Alert { Level = 2, Title = "Many failed logins" };
Alert second = new Alert(3) { Title = "Disk full", Source = "monitor" };
Console.WriteLine(first.Describe());
Console.WriteLine(second.Describe());

class Alert
{
    private int level = 1;
    private string title = "";
    public string Source = "manual";

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
        set
        {
            if (value >= 1 && value <= 3)
            {
                level = value;
            }
        }
    }

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public string Describe() => $"{Title} (level {Level}, {Source})";
}
