Point a = new(1, 2);
Point b = a;
b.X = 10;
Console.WriteLine(a.X);
Console.WriteLine(b.X);

Move(a);
Console.WriteLine(a.X);

void Move(Point p)
{
    p.X = 99;
}

struct Point
{
    public int X { get; set; }
    public int Y { get; set; }

    public Point(int x, int y)
    {
        X = x;
        Y = y;
    }
}
