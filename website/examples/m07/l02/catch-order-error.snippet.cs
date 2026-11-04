try
{
    Console.WriteLine(int.Parse("abc"));
}
catch (Exception ex)
{
    Console.WriteLine($"General: {ex.GetType().Name}");
}
catch (FormatException)
{
    Console.WriteLine("Not a number");
}
