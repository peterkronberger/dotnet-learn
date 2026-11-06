Row row = new("alice", "bob", "carol");
Console.WriteLine(row[0]);
Console.WriteLine(row[2]);
row[1] = "dave";
Console.WriteLine(row[1]);

class Row(params string[] names)
{
    private readonly string[] items = names;

    public string this[int index]
    {
        get { return items[index]; }
        set { items[index] = value; }
    }
}
