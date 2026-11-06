List<string> names = ["alice", "bob", "carol"];

IPicker<string> first = new FirstPicker<string>();
Console.WriteLine(first.Pick(names));
// Task 1: write the class LastPicker<T> that implements IPicker<T>
// and returns the last item
// Task 2: use LastPicker<string> through an IPicker<string> variable
// and print the result for names

interface IPicker<T>
{
    T Pick(IReadOnlyList<T> items);
}

class FirstPicker<T> : IPicker<T>
{
    public T Pick(IReadOnlyList<T> items)
    {
        return items[0];
    }
}
