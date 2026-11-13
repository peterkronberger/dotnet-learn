outer:
for (int row = 1; row <= 3; row++)
{
    for (int column = 1; column <= 3; column++)
    {
        if (row == 2 && column == 2)
        {
            break outer;
        }
        Console.WriteLine($"row {row}, column {column}");
    }
}
Console.WriteLine("done");
