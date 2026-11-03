Alert alert = new Alert();
alert.Title = "  Disk full  ";
Console.WriteLine($"[{alert.Title}]");

class Alert
{
    public string Title { get; set; } = "";
}
