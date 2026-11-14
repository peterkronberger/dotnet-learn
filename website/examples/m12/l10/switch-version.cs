// A closed hierarchy: each operation is one switch
Shape[] shapes = [new Square(3), new Rectangle(2, 5)];

foreach (Shape shape in shapes)
{
    Console.WriteLine(
        $"{shape.GetType().Name}: area {Area(shape)}, "
        + $"perimeter {Perimeter(shape)}");
}

int Area(Shape shape) => shape switch
{
    Square square => square.Side * square.Side,
    Rectangle rectangle => rectangle.Width * rectangle.Height,
};

// A new operation: the shape types stay unchanged
int Perimeter(Shape shape) => shape switch
{
    Square square => 4 * square.Side,
    Rectangle rectangle => 2 * (rectangle.Width + rectangle.Height),
};

closed record Shape;
sealed record Square(int Side) : Shape;
sealed record Rectangle(int Width, int Height) : Shape;
