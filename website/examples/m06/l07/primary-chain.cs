Alert first = new Alert("Many failed logins", 3);
Alert second = new Alert("Disk full");
Console.WriteLine(first.Describe());
Console.WriteLine(second.Describe());

class Alert(string title, int level)
{
    public Alert(string title) : this(title, 1)
    {
        Console.WriteLine($"one parameter: {title}");
    }

    public string Describe()
    {
        return $"{title}: level {level}";
    }
}
