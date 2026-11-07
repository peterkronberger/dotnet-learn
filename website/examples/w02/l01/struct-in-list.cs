var points = new List<Point> { new Point { X = 1 }, new Point { X = 2 } };

Point copy = points[0];
copy.X = 10;
Console.WriteLine(points[0].X);

points[1] = new Point { X = 20 };
Console.WriteLine(points[1].X);

var boxes = new List<Box> { new Box { X = 1 } };
Box same = boxes[0];
same.X = 10;
Console.WriteLine(boxes[0].X);

struct Point
{
    public int X;
}

class Box
{
    public int X;
}
