Console.WriteLine("Create the alert");
Alert alert = new Alert(1) { Title = "Many failed logins", Level = 3 };
Console.WriteLine("Done");

class Alert
{
    private int level;
    private string title = "";

    public Alert(int level)
    {
        this.level = level;
        Console.WriteLine($"constructor runs with level {level}");
    }

    public int Level
    {
        get { return level; }
        set
        {
            Console.WriteLine($"Level set to {value}");
            level = value;
        }
    }

    public string Title
    {
        get { return title; }
        set
        {
            Console.WriteLine($"Title set to {value}");
            title = value;
        }
    }
}
