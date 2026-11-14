// A slice pattern .. stands for zero or more elements
int[] numbers = [10, 20, 30, 40];
Console.WriteLine(numbers is [10, ..]);
Console.WriteLine(numbers is [.., 40]);
Console.WriteLine(numbers is [10, .., 40]);
Console.WriteLine(numbers is [10, 20, 30, 40, ..]);
Console.WriteLine(numbers is [10, 20, 30, 40, 50, ..]);

// .. var rest keeps the skipped elements
if (numbers is [var head, .. var rest])
{
    Console.WriteLine($"head = {head}, rest has {rest.Length} elements");
    Console.WriteLine(rest.GetType().Name);
}

// On a string the slice is a string again
string text = "user=bob";
if (text is ['u', 's', 'e', 'r', '=', .. var name])
{
    Console.WriteLine($"{name} {name.GetType().Name}");
}
