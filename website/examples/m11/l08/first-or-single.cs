string[] ids = ["a1", "b2", "c3", "b2"];

Console.WriteLine($"First b2: {ids.First(id => id == "b2")}");
Console.WriteLine($"Single a1: {ids.Single(id => id == "a1")}");
try
{
    ids.Single(id => id == "b2");
}
catch (InvalidOperationException ex)
{
    Console.WriteLine($"Single b2: {ex.GetType().Name}");
}
