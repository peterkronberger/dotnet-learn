ClassPoint c1 = new(1, 2);
ClassPoint c2 = c1;
StructPoint s1 = new(1, 2);
StructPoint s2 = s1;
s2.X = 9;
ReadonlyPoint r1 = new(1, 2);

Console.WriteLine(ReferenceEquals(c1, c2));
Console.WriteLine($"{s1.X} {s2.X}");
Console.WriteLine(c1);
Console.WriteLine(s1);
Console.WriteLine(r1);

record ClassPoint(int X, int Y);

record struct StructPoint(int X, int Y);

readonly record struct ReadonlyPoint(int X, int Y);
