// A closed record: the switch needs no discard arm
Shape first = new Circle(2);
Shape second = new Square(3);

Console.WriteLine(Describe(first));
Console.WriteLine(Describe(second));
Console.WriteLine($"Shape is abstract: {typeof(Shape).IsAbstract}");

// One arm per direct derived type, no _ arm
string Describe(Shape shape) => shape switch
{
    Circle circle => $"circle with radius {circle.Radius}",
    Square square => $"square with side {square.Side}",
};

closed record Shape;
sealed record Circle(int Radius) : Shape;
sealed record Square(int Side) : Shape;
