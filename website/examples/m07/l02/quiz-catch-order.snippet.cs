try
{
    int number = int.Parse("12x");
    Console.WriteLine(number);
}
catch (Exception ex)
{
    Console.WriteLine("Error: " + ex.Message);
}
catch (FormatException)
{
    Console.WriteLine("Not a number");
}
