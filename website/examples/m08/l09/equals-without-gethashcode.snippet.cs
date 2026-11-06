class HalfSeat(int row, int number)
{
    public int Row => row;
    public int Number => number;

    public override bool Equals(object? obj)
    {
        return obj is HalfSeat other && Row == other.Row && Number == other.Number;
    }
}
