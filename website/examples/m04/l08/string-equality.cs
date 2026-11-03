using System.Text;

string literal = "bob";
string built = new StringBuilder("bob").ToString();
Console.WriteLine(literal == built);
Console.WriteLine(literal.Equals(built));
Console.WriteLine(object.ReferenceEquals(literal, built));
Console.WriteLine(literal == "BOB");
Console.WriteLine(literal.ToUpper() == "BOB");
