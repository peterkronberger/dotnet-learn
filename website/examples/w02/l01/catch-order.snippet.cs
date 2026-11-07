try
{
    int.Parse("abc");
}
catch (Exception)
{
    Console.WriteLine("Something failed");
}
catch (FormatException)
{
    Console.WriteLine("Not a number");
}
