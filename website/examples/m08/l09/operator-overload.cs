Seat first = new(2, 5);
Seat second = new(2, 5);

Console.WriteLine(first == second);
Console.WriteLine(first != second);
Console.WriteLine(first.Equals(second));

class Seat(int row, int number)
{
    public int Row => row;
    public int Number => number;

    public static bool operator ==(Seat? left, Seat? right)
    {
        if (left is null || right is null)
        {
            return left is null && right is null;
        }
        return left.Row == right.Row && left.Number == right.Number;
    }

    public static bool operator !=(Seat? left, Seat? right)
    {
        return !(left == right);
    }

    public override bool Equals(object? obj)
    {
        return this == obj as Seat;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Number);
    }
}
