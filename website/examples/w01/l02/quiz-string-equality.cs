using System.Text;

string a = new StringBuilder("ab").ToString();
string b = new StringBuilder("ab").ToString();
Console.WriteLine(a == b);
Console.WriteLine(object.ReferenceEquals(a, b));
