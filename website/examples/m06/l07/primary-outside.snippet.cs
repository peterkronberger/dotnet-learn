Alert alert = new Alert("Disk full", 1);
Console.WriteLine(alert.title);

class Alert(string title, int level)
{
    public string Describe() => $"{title}: level {level}";
}
