HashSet<HalfSeat> half = [new HalfSeat(2, 5), new HalfSeat(2, 5)];
Console.WriteLine($"Without GetHashCode: {half.Count}");
Console.WriteLine(half.Contains(new HalfSeat(2, 5)));

HashSet<FullSeat> full = [new FullSeat(2, 5), new FullSeat(2, 5)];
Console.WriteLine($"With GetHashCode: {full.Count}");
Console.WriteLine(full.Contains(new FullSeat(2, 5)));

#pragma warning disable CS0659
class HalfSeat(int row, int number)
{
    public int Row => row;
    public int Number => number;

    public override bool Equals(object? obj)
    {
        return obj is HalfSeat other && Row == other.Row && Number == other.Number;
    }
}

#pragma warning restore CS0659
class FullSeat(int row, int number)
{
    public int Row => row;
    public int Number => number;

    public override bool Equals(object? obj)
    {
        return obj is FullSeat other && Row == other.Row && Number == other.Number;
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Row, Number);
    }
}
