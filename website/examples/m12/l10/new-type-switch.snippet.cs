int Area(Shape shape) => shape switch
{
    Square square => square.Side * square.Side,
    Rectangle rectangle => rectangle.Width * rectangle.Height,
};

int Perimeter(Shape shape) => shape switch
{
    Square square => 4 * square.Side,
    Rectangle rectangle => 2 * (rectangle.Width + rectangle.Height),
};

closed record Shape;
sealed record Square(int Side) : Shape;
sealed record Rectangle(int Width, int Height) : Shape;
sealed record Triangle(int Base, int Height) : Shape;
