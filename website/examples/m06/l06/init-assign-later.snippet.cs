Alert alert = new() { Title = "Disk full" };
alert.Title = "Many failed logins";

class Alert
{
    public required string Title { get; init; }
}
