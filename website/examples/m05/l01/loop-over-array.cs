int[] failures = { 1, 5, 0, 1 };
int total = 0;
for (int i = 0; i < failures.Length; i++)
{
    total += failures[i];
}
Console.WriteLine($"for: {total}");
int sum = 0;
foreach (int count in failures)
{
    sum += count;
}
Console.WriteLine($"foreach: {sum}");
