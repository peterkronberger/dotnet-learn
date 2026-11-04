Console.WriteLine("Start");
try
{
    Console.WriteLine("Step 1");
    int number = int.Parse("12x");
    Console.WriteLine("Step 2");
}
catch (FormatException)
{
    Console.WriteLine("Step 3");
}
Console.WriteLine("Step 4");
