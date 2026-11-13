FailureCounts counts = new([1, 5, 0, 1]);
Console.WriteLine(counts[1]);
Console.WriteLine(counts[3]);

class FailureCounts(int[] values)
{
    public int[] Values => values;
}

static class FailureCountsExtensions
{
    extension(FailureCounts counts)
    {
        public int this[int index] => counts.Values[index];
    }
}
