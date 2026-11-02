Console.WriteLine(Sum());
Console.WriteLine(Sum(5));
Console.WriteLine(Sum(1, 2, 3));

int Sum(params int[] numbers)
{
    int total = 0;
    foreach (int number in numbers)
    {
        total += number;
    }
    return total;
}
