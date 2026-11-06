int[] numbers = [1, 2, 3];

// The caller decides what happens to each number
Print(Change(numbers, Double));
Print(Change(numbers, Negate));

static int[] Change(int[] values, Func<int, int> change)
{
    int[] result = new int[values.Length];
    for (int i = 0; i < values.Length; i++)
    {
        result[i] = change(values[i]);
    }
    return result;
}

static int Double(int number)
{
    return number * 2;
}

static int Negate(int number)
{
    return -number;
}

static void Print(int[] values)
{
    Console.WriteLine(string.Join(", ", values));
}
