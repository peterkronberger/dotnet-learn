Counter<int> a = new();
Counter<int> b = new();
Counter<string> c = new();
Counter<int> d = new();

Console.WriteLine(Counter<int>.Created);
Console.WriteLine(Counter<string>.Created);

class Counter<T>
{
    public static int Created;

    public Counter()
    {
        Created++;
    }
}
