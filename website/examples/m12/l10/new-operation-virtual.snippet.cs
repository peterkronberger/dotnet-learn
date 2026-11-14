abstract record Shape
{
    public abstract int Area();
    public abstract int Perimeter();
}

sealed record Square(int Side) : Shape
{
    public override int Area() => Side * Side;
}

sealed record Rectangle(int Width, int Height) : Shape
{
    public override int Area() => Width * Height;
}
