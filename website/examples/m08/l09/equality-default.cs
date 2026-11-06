Ticket first = new(7, "printer");
Ticket second = new(7, "printer");
Ticket alias = first;

Console.WriteLine(first == second);
Console.WriteLine(first.Equals(second));
Console.WriteLine(first == alias);

Slot slotA = new(3, 4);
Slot slotB = new(3, 4);
Console.WriteLine(slotA.Equals(slotB));

class Ticket(int id, string title)
{
    public int Id => id;
    public string Title => title;
}

struct Slot(int row, int column)
{
    public int Row => row;
    public int Column => column;
}
