using System.Text;

string joined = "";
var builder = new StringBuilder();
for (int number = 1; number <= 3; number++)
{
    joined = joined + number + ";";
    builder.Append(number);
    builder.Append(";");
}
Console.WriteLine(joined);
Console.WriteLine(builder.ToString());
Console.WriteLine(builder.Length);
