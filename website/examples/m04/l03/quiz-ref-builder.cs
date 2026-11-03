using System.Text;

var a = new StringBuilder("a");
var b = new StringBuilder("b");
Swap(ref a, ref b);
Console.WriteLine($"{a} {b}");

void Swap(ref StringBuilder left, ref StringBuilder right)
{
    var temp = left;
    left = right;
    right = temp;
}
