Shape shape = new Square(3);
Console.WriteLine(Area(shape));

shape = new Rectangle(2, 5);
Console.WriteLine(Area(shape));

int Area(Shape value) => value switch
{
    Square square => square.Side * square.Side,
    Rectangle rectangle => rectangle.Width * rectangle.Height,
};

record Square(int Side);
record Rectangle(int Width, int Height);
union Shape(Square, Rectangle);
