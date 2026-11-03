using System.Text;

var first = new StringBuilder("bob");
var second = new StringBuilder("bob");
var third = first;
Console.WriteLine(first == second);
Console.WriteLine(first == third);
Console.WriteLine(object.ReferenceEquals(first, second));
Console.WriteLine(object.ReferenceEquals(first, third));
