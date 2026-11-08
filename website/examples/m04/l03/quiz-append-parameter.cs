using System.Text;

var log = new StringBuilder("old");
AddMark(log);
Console.WriteLine(log);

void AddMark(StringBuilder builder)
{
    builder.Append("!");
}
