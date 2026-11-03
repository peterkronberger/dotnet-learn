Alert alert = new Alert();
alert.Level = 2;
alert.Level = 9;
Console.WriteLine(alert.Level);

class Alert
{
    private int level = 1;

    // Only the levels 1 to 3 are allowed
    public int Level
    {
        get { return level; }
        set { level = value; }
    }
}
