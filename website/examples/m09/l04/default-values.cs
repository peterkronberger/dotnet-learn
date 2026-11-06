Show<int>();
Show<bool>();
Show<string>();
Show<Point>();

int number = default;
bool flag = default;
Point zero = default;
Console.WriteLine($"{number} {flag} {zero.X} {zero.Y}");

static void Show<T>()
{
    T? viaType = default(T);
    T? viaLiteral = default;
    bool same = Equals(viaType, viaLiteral);
    Console.WriteLine($"{typeof(T).Name}: null={viaType is null}, {same}");
}

struct Point
{
    public int X { get; set; }
    public int Y { get; set; }
}
