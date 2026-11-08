using System.Text;

StringBuilder? findings = null;
findings?.Length = NextLength();
Console.WriteLine("done");

int NextLength()
{
    Console.WriteLine("NextLength called");
    return 0;
}
