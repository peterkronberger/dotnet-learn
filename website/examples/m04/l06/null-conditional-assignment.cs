using System.Text;

StringBuilder? findings = new StringBuilder("bob");
StringBuilder? none = null;
findings?.Length = NextLength();
none?.Length = NextLength();
Console.WriteLine(findings);
Console.WriteLine(none == null);

int NextLength()
{
    Console.WriteLine("NextLength called");
    return 2;
}
