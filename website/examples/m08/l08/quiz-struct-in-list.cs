List<Point> points = new() { new Point(1, 2) };
Point p = points[0];
p.X = 10;
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
