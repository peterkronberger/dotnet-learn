// The same range test as a comparison chain and as a pattern
int[] failures = [0, 1, 2, 3, 5];
foreach (int count in failures)
{
    bool withOperators = count >= 1 && count < 3;
    bool withPattern = count is >= 1 and < 3;
    Console.WriteLine($"{count}: {withOperators} {withPattern}");
}
