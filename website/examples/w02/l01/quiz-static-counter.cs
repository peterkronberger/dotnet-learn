new Roster();
new Roster();
Console.WriteLine(Roster.Created);

class Roster
{
    public static int Created;

    public Roster()
    {
        Created++;
    }
}
