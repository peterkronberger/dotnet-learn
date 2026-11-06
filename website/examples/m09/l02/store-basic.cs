Store<int> numbers = new();
numbers.Add(10);
numbers.Add(20);
int second = numbers.Get(1);
Console.WriteLine($"{numbers.Count} numbers, second: {second}");

Store<string> words = new();
words.Add("ten");
string first = words.Get(0);
Console.WriteLine($"{words.Count} words, first: {first}");

class Store<T>
{
    private readonly List<T> items = [];

    public int Count => items.Count;

    public void Add(T item)
    {
        items.Add(item);
    }

    public T Get(int index)
    {
        return items[index];
    }
}
