Func<int, bool> isEven = number =>
{
    Console.WriteLine($"Lambda runs with {number}");
    return number % 2 == 0;
};
Console.WriteLine("Lambda created");

Console.WriteLine(isEven(4));
Console.WriteLine(isEven(7));
