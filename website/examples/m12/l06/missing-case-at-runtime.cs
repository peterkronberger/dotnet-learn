// A non-exhaustive switch expression throws at run time
// The compiler warns about it (the lesson shows this)
#pragma warning disable CS8524
foreach (Color color in new[] { Color.Red, (Color)7 })
{
    try
    {
        string name = color switch
        {
            Color.Red => "red",
            Color.Green => "green",
            Color.Blue => "blue",
        };
        Console.WriteLine(name);
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.GetType().Name);
    }
}

enum Color { Red, Green, Blue }
