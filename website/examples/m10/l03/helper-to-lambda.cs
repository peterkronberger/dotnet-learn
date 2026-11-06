List<int> numbers = [8, 3, 10, 5, 4];

// With a helper method: the name is a method group
Console.WriteLine(string.Join(", ", numbers.FindAll(IsEven)));

// With a lambda: the same check, written where it is needed
Console.WriteLine(string.Join(", ", numbers.FindAll(n => n % 2 == 0)));

static bool IsEven(int number)
{
    return number % 2 == 0;
}
