for (int row = 1; row <= 3; row++)
{
    for (int column = 1; column <= 3; column++)
    {
        if (column == 2)
        {
            break;
        }
        Console.WriteLine($"row {row}, column {column}");
    }
}
Console.WriteLine("done");
