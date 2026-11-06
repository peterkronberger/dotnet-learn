class Seat(int row, int number)
{
    public int Row => row;
    public int Number => number;

    public static bool operator ==(Seat? left, Seat? right)
    {
        return left?.Row == right?.Row && left?.Number == right?.Number;
    }
}
