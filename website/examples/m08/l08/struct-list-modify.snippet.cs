List<Point> points = [new Point(1, 2)];
points[0].X = 10;

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
