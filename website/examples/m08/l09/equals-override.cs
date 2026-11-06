Seat first = new(2, 5);
Seat second = new(2, 5);
Seat other = new(2, 6);

Console.WriteLine(first.Equals(second));
Console.WriteLine(first.Equals(other));
Console.WriteLine(first.GetHashCode() == second.GetHashCode());
Console.WriteLine(first == second);
Console.WriteLine(first.Equals(null));

class Seat : IEquatable<Seat>
{
    public int Row { get; }
    public int Number { get; }

    public Seat(int row, int number)
    {
        Row = row;
        Number = number;
    }

    public bool Equals(Seat? other)
    {
        return other is not null && Row == other.Row && Number == other.Number;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as Seat);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Number);
    }
}
