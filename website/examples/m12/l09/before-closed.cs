// Before C# 15: an abstract base record and a discard arm
Shape first = new Circle(2);
Shape second = new Square(3);

Console.WriteLine(Describe(first));
Console.WriteLine(Describe(second));

string Describe(Shape shape) => shape switch
{
    Circle circle => $"circle with radius {circle.Radius}",
    Square square => $"square with side {square.Side}",
    _ => "unknown shape",
};

abstract record Shape;
record Circle(int Radius) : Shape;
record Square(int Side) : Shape;
