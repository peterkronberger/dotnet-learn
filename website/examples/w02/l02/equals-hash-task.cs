HashSet<Seat> seats = [new Seat(2, 5), new Seat(2, 5)];
Console.WriteLine(seats.Count);

#pragma warning disable CS0659
class Seat(int row, int number)
{
    public int Row => row;
    public int Number => number;

    // TODO: add GetHashCode so that equal seats have the same hash code
    public override bool Equals(object? obj)
    {
        return obj is Seat other && Row == other.Row && Number == other.Number;
    }
}
