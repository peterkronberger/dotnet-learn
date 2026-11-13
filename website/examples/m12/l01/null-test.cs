// Which tests are true for a variable that holds null?
string? text = null;
object? boxed = text;
Console.WriteLine(text is string);
Console.WriteLine(boxed is string);
Console.WriteLine(text is null);
Console.WriteLine(text is not null);
