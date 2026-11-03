using System.Text;

var one = new StringBuilder("ab");
var two = new StringBuilder("ab");
string textOne = one.ToString();
string textTwo = two.ToString();
Console.WriteLine(one == two);
Console.WriteLine(textOne == textTwo);
Console.WriteLine(object.ReferenceEquals(textOne, textTwo));
two = one;
Console.WriteLine(one == two);
