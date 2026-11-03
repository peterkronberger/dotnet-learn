using System.Text;

int a = 1;
int b = a;
b++;
var x = new StringBuilder("1");
var y = x;
y.Append("1");
Console.WriteLine($"{a} {b}");
Console.WriteLine($"{x} {y}");
