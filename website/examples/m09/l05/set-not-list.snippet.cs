HashSet<int> set = [3, 1, 2];
Console.WriteLine(FirstOf(set));

static int FirstOf(IReadOnlyList<int> numbers)
{
    return numbers[0];
}
