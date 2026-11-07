Shape shape = new Square();

Console.WriteLine(shape is Circle);
Circle? asCircle = shape as Circle;
Console.WriteLine(asCircle is null);

try
{
    Circle circle = (Circle)shape;
    Console.WriteLine("cast worked");
}
catch (InvalidCastException)
{
    Console.WriteLine("cast failed");
}

Console.WriteLine(shape.GetType().Name);

class Shape
{
}

class Square : Shape
{
}

class Circle : Shape
{
}
