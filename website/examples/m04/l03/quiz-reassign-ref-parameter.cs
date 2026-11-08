using System.Text;

var log = new StringBuilder("old");
Replace(ref log);
Console.WriteLine(log);

void Replace(ref StringBuilder builder)
{
    builder = new StringBuilder("new");
}
