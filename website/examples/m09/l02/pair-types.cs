Pair<int> point = new(3, 4);
Console.WriteLine($"{point.First} {point.Second}");

Entry<string, int> entry = new("bob", 5);
Console.WriteLine(entry);

Entry<string, double> rate = entry.WithValue(71.4);
Console.WriteLine(rate);

struct Pair<T>
{
    public T First { get; }
    public T Second { get; }

    public Pair(T first, T second)
    {
        First = first;
        Second = second;
    }
}

record Entry<TKey, TValue>(TKey Key, TValue Value)
{
    public Entry<TKey, TOther> WithValue<TOther>(TOther other)
    {
        return new Entry<TKey, TOther>(Key, other);
    }
}
