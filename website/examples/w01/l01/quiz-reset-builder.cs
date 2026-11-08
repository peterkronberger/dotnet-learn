using System.Text;

var log = new StringBuilder("old");
Reset(log);
Console.WriteLine(log);

void Reset(StringBuilder text)
{
    text = new StringBuilder("new");
}
