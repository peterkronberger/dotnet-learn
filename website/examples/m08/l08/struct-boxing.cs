Point point = new(1, 2);

object boxed = point;
point.X = 5;
Console.WriteLine(((Point)boxed).X);

IMovable movable = point;
movable.MoveRight();
Console.WriteLine(point.X);
Console.WriteLine(((Point)movable).X);

interface IMovable
{
    void MoveRight();
}

struct Point : IMovable
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }

    public void MoveRight()
    {
        X++;
    }
}
