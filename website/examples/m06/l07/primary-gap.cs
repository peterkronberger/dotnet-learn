Alert alert = new Alert("Disk full", 2);
Console.WriteLine(alert.Describe());

class Alert
{
    private readonly string title;
    private readonly int level;

    public Alert(string title, int level)
    {
        this.title = title;
        this.level = level;
    }

    public string Describe()
    {
        return $"{title}: level {level}";
    }
}
