int[] failuresPerHour = [1, 5, 0, 3];
int total = SumFailures(failuresPerHour);
Console.WriteLine($"Failures in total: {total}");
Console.WriteLine($"Expected: {1 + 5 + 0 + 3}");

int SumFailures(int[] values)
{
    int sum = 0;
    for (int i = 1; i < values.Length; i++)
    {
        sum += values[i];
    }
    return sum;
}
