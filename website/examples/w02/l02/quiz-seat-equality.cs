Seat a = new(2, 5);
Seat b = new(2, 5);
Console.WriteLine(a == b);

class Seat(int row, int number)
{
    public int Row => row;
    public int Number => number;
}
