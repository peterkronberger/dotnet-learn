foreach (string text in Repeat("ok", 2))
{
    Console.WriteLine(text);
}

static IEnumerable<T> Repeat<T>(T item, int times)
{
    for (int i = 0; i < times; i++)
    {
        Console.WriteLine("making");
        yield return item;
    }

    Console.WriteLine("end");
}
