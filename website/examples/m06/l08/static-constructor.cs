Console.WriteLine("Program starts");
Marker first = new Marker();
Marker second = new Marker();
Console.WriteLine("Two markers created");

class Marker
{
    static Marker()
    {
        Console.WriteLine("Static constructor runs");
    }

    public Marker()
    {
        Console.WriteLine("Instance constructor runs");
    }
}
