// Explicit parameter types: the lambda has a natural type
var twice = (int x) => x * 2;
var show = (string text) => Console.WriteLine($"> {text}");

// Explicit return type: written before the parameter list
var parse = int? (string text) =>
    int.TryParse(text, out int value) ? value : null;

// A method group with one overload has a natural type too
var square = Square;

Console.WriteLine(twice(21));
show("natural type");
Console.WriteLine(parse("42"));
Console.WriteLine(parse("x") is null);
Console.WriteLine(square(7));

static int Square(int number)
{
    return number * number;
}
