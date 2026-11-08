Ticket a = new Ticket("Disk full", 3);
Ticket b = new Ticket("Disk full", 3);
Console.WriteLine(a == b);

class Ticket
{
    public string Title { get; }
    public int Level { get; }

    public Ticket(string title, int level)
    {
        Title = title;
        Level = level;
    }
}
