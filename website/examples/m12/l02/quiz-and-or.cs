int[] values = [1, 4, 8];
foreach (int value in values)
{
    Console.WriteLine(value is < 2 or > 6 and < 9);
}
