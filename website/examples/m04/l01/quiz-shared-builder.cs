using System.Text;

var a = new StringBuilder("x");
var b = a;
b.Append("y");
Console.WriteLine(a);
