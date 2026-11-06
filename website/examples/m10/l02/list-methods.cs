List<int> numbers = [8, 3, 10, 5, 4];

// Predicate<int>: find, filter and remove
Console.WriteLine(numbers.Find(IsBig));
Console.WriteLine(string.Join(", ", numbers.FindAll(IsEven)));
int removed = numbers.RemoveAll(IsEven);
Console.WriteLine($"Removed {removed}, left: {numbers.Count}");

// Comparison<int>: sort the rest in descending order
numbers.Sort(Descending);

// Action<int>: run code for each item
numbers.ForEach(Console.WriteLine);

static bool IsBig(int number)
{
    return number >= 9;
}

static bool IsEven(int number)
{
    return number % 2 == 0;
}

static int Descending(int left, int right)
{
    return right.CompareTo(left);
}
