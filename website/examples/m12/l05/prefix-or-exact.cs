// Does [1, 2] mean "begins with 1, 2" or "is exactly 1, 2"?
int[][] samples = [[1, 2], [1, 2, 3], [1]];
foreach (int[] sample in samples)
{
    bool exact = sample is [1, 2];
    bool prefix = sample is [1, 2, ..];
    Console.WriteLine($"{sample.Length}: {exact} {prefix}");
}
