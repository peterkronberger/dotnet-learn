using System.Text;

var text = new StringBuilder("old");
Change(text);
Console.WriteLine(text);

void Change(StringBuilder builder)
{
    builder.Append("+more");
}
