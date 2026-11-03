int sum = 0;
for (int i = 1; i <= 6; i++)
{
    if (i % 3 == 0)
    {
        continue;
    }
    sum += i;
}
Console.WriteLine($"Sum: {sum}");
