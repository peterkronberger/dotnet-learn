string Describe(Shape shape) => shape switch
{
    Circle circle => $"circle with radius {circle.Radius}",
};

closed record Shape;
sealed record Circle(int Radius) : Shape;
sealed record Square(int Side) : Shape;
