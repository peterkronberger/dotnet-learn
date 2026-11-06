Store<int> numbers = new();
numbers.Add("ten");
Store names = new();

class Store<T>
{
    private readonly List<T> items = [];

    public void Add(T item)
    {
        items.Add(item);
    }
}
