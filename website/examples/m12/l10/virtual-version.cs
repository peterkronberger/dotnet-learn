// Polymorphism: every type brings its own Area
Shape[] shapes = [new Square(3), new Rectangle(2, 5)];

foreach (Shape shape in shapes)
{
    Console.WriteLine($"{shape.GetType().Name}: area {shape.Area()}");
}

abstract record Shape
{
    public abstract int Area();
}

sealed record Square(int Side) : Shape
{
    public override int Area() => Side * Side;
}

sealed record Rectangle(int Width, int Height) : Shape
{
    public override int Area() => Width * Height;
}
