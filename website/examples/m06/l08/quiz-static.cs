Visitor a = new Visitor();
Visitor b = new Visitor();
a.Visit();
a.Visit();
b.Visit();
Console.WriteLine($"{a.Own} {b.Own} {Visitor.Total}");

class Visitor
{
    public static int Total;
    public int Own;

    public void Visit()
    {
        Own++;
        Total++;
    }
}
