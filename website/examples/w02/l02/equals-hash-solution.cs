HashSet<Seat> seats = [new Seat(2, 5), new Seat(2, 5)];
Console.WriteLine(seats.Count);

class Seat(int row, int number)
{
    public int Row => row;
    public int Number => number;

    public override bool Equals(object? obj)
    {
        return obj is Seat other && Row == other.Row && Number == other.Number;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Number);
    }
}
