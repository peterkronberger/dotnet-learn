using System.Text;

var log = new StringBuilder("old");
Replace(log);
Console.WriteLine(log);

void Replace(StringBuilder builder)
{
    builder = new StringBuilder("new");
}
