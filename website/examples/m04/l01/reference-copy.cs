using System.Text;

var first = new StringBuilder("alice");
var second = first;
second.Append("!");
Console.WriteLine($"first: {first}");
Console.WriteLine($"second: {second}");
