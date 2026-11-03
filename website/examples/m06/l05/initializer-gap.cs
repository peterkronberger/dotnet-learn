Alert alert = new Alert(3) { Source = "monitor" };
Console.WriteLine(alert.Describe());

class Alert
{
    private int level;
    private string title = "";
    public string Source = "manual";

    public Alert(int level)
    {
        this.level = level;
    }

    public string Title
    {
        get { return title; }
        set { title = value; }
    }

    public string Describe() => $"{title} (level {level}, {Source})";
}
