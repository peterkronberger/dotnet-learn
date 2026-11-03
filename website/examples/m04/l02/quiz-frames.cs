using System.Text;

var report = new StringBuilder("bob");
Mark(report);
Console.WriteLine(report);

void Mark(StringBuilder target)
{
    var alias = target;
    alias.Append("!");
}
