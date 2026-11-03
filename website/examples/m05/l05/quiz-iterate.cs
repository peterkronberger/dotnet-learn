List<int> numbers = new List<int>();
numbers.Add(1);
numbers.Add(2);
numbers.Add(2);
numbers.Add(3);

for (int i = numbers.Count - 1; i >= 0; i--)
{
    if (numbers[i] == 2)
    {
        numbers.RemoveAt(i);
    }
}

Console.WriteLine(numbers.Count);
Console.WriteLine(numbers[1]);
