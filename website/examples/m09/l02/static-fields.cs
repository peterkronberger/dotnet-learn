Store<int> a = new();
Store<int> b = new();
Store<string> c = new();

Console.WriteLine(Store<int>.Created);
Console.WriteLine(Store<string>.Created);
Console.WriteLine(a.GetType() == b.GetType());
Console.WriteLine(a.GetType() == c.GetType());

class Store<T>
{
    public static int Created;

    public Store()
    {
        Created++;
    }
}
