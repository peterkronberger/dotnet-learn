int[,] table = { { 1, 2, 3 }, { 4, 5, 6 } };
int sum = 0;
// The loops should visit every element, not only the first row
for (int row = 0; row < 1; row++)
{
    for (int column = 0; column < table.GetLength(1); column++)
    {
        sum += table[row, column];
    }
}
Console.WriteLine(sum);
