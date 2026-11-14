// A new type of shape: only a new record, no existing code changes
Shape[] shapes = [new Square(3), new Triangle(4, 5)];

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

sealed record Triangle(int Base, int Height) : Shape
{
    public override int Area() => Base * Height / 2;
}
