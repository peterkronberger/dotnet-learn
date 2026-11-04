Alert first = new Alert("Many failed logins", 3);
Alert second = new Alert("Disk full", 1);
Console.WriteLine(first.Describe());
Console.WriteLine(second.Describe());

class Alert(string title, int level)
{
    public string Describe()
    {
        return $"{title}: level {level}";
    }
}
