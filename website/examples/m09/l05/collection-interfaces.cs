int[] array = [3, 1, 2];
List<int> list = [3, 1, 2];
HashSet<int> set = [3, 1, 2];

Console.WriteLine(Sum(array));
Console.WriteLine(Sum(list));
Console.WriteLine(Sum(set));
Console.WriteLine(FirstOf(array));
Console.WriteLine(FirstOf(list));

static int Sum(IEnumerable<int> numbers)
{
    int total = 0;
    foreach (int number in numbers)
    {
        total += number;
    }
    return total;
}

static int FirstOf(IReadOnlyList<int> numbers)
{
    return numbers[0];
}
