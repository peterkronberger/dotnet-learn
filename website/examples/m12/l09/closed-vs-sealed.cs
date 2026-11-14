// closed allows derived types, sealed forbids them
Console.WriteLine($"Shape is abstract: {typeof(Shape).IsAbstract}");
Console.WriteLine($"Shape is sealed: {typeof(Shape).IsSealed}");
Console.WriteLine($"Circle is sealed: {typeof(Circle).IsSealed}");
Console.WriteLine($"Polygon is sealed: {typeof(Polygon).IsSealed}");

// Triangle derives from Polygon, so the Polygon arm covers it
Shape shape = new Triangle(3);
string text = shape switch
{
    Circle => "circle",
    Polygon => "polygon",
};
Console.WriteLine(text);

closed record Shape;
sealed record Circle(int Radius) : Shape;
record Polygon : Shape;
sealed record Triangle(int Side) : Polygon;
