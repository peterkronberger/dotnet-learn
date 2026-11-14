// The empty property pattern tests for "not null"
object?[] values = [null, "", 0, "bob"];
foreach (object? value in values)
{
    Console.WriteLine($"{value is { }} {value is not { }}");
}
