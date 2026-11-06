Point first = new(3, 4);
Point second = new(3, 4);

Console.WriteLine(first);
Console.WriteLine(first == second);
Console.WriteLine(first.Equals(second));
Console.WriteLine(first.X + first.Y);

var (x, y) = first;
Console.WriteLine($"x={x}, y={y}");

record Point(int X, int Y);
