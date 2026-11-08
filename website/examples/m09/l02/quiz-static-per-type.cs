Store<int>.Created = 1;
Store<string>.Created = 2;

Console.WriteLine(Store<int>.Created);
Console.WriteLine(Store<string>.Created);

class Store<T>
{
    public static int Created;
}
