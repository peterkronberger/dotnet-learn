using System.Text;

var text = new StringBuilder("log");
int times = 2;
AddTimes(text, times);
Console.WriteLine(text);

void AddTimes(StringBuilder builder, int count)
{
    builder.Append(count);
}
