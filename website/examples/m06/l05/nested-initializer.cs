Report report = new Report { Main = { Level = 3 } };
Console.WriteLine(report.Main.Level);

class Report
{
    private readonly Alert main = new Alert();

    public Alert Main
    {
        get { return main; }
    }
}

class Alert
{
    private int level = 1;

    public int Level
    {
        get { return level; }
        set { level = value; }
    }
}
