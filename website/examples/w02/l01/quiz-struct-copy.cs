var points = new List<Point> { new Point { X = 1 } };

Point copy = points[0];
copy.X = 10;
Console.WriteLine(points[0].X);

struct Point
{
    public int X;
}
