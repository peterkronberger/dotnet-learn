// A list pattern matches the length exactly and each element in order
int[] two = [1, 2];
int[] three = [1, 2, 3];
List<int> list = [1, 2];
int[] none = [];
Console.WriteLine(two is [1, 2]);
Console.WriteLine(three is [1, 2]);
Console.WriteLine(list is [1, 2]);
Console.WriteLine(none is []);

// Elements go into variables; every subpattern is a pattern
if (three is [var first, > 1, _])
{
    Console.WriteLine($"first = {first}");
}

// A list pattern never matches null
int[]? missing = null;
Console.WriteLine(missing is []);
Console.WriteLine(missing is not [1, 2]);
