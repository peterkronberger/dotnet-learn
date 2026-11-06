Point first = new(1, 2);
Point second = new(1, 2);
HashSet<Point> points = [first, second];
Console.WriteLine(first.Equals(second));
Console.WriteLine(points.Count);

class Point(int x, int y)
{
    public int X => x;
    public int Y => y;

    public override bool Equals(object? obj)
    {
        // Gap 1: return true if obj is a Point with the same X and Y
        return false;
    }

    public override int GetHashCode()
    {
        // Gap 2: return a hash code built from X and Y (HashCode.Combine)
        return 0;
    }
}
