List<string> words = ["pear", "fig", "apple", "kiwi"];
// Task 2: sort the list with a new LengthComparer
Console.WriteLine(string.Join(", ", words));

class LengthComparer : IComparer<string>
{
    public int Compare(string? x, string? y)
    {
        if (x is null || y is null)
        {
            return 0;
        }
        // Task 1: shorter words first; for the same length use string.CompareOrdinal
        return 0;
    }
}
