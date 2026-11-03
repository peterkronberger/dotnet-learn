Box box = new Box();
Console.WriteLine(box.Size);

class Box
{
    public int Size;

    public Box(int size)
    {
        Console.WriteLine("A");
        Size = size;
    }

    public Box() : this(1)
    {
        Console.WriteLine("B");
    }
}
