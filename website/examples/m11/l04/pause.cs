static IEnumerable<string> Steps()
{
    Console.WriteLine("start of the body");
    yield return "one";
    Console.WriteLine("after the first yield return");
    yield return "two";
    Console.WriteLine("end of the body");
}

IEnumerable<string> steps = Steps();
Console.WriteLine("iterator created");

foreach (string step in steps)
{
    Console.WriteLine($"got {step}");
}
