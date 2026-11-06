List<Point> points = [new Point(1, 2)];

Point copy = points[0];
copy.X = 10;
Console.WriteLine(points[0].X);
Console.WriteLine(copy.X);

points[0] = copy;
Console.WriteLine(points[0].X);

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
