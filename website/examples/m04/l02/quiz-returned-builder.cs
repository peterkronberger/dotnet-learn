using System.Text;

var result = Create();
Console.WriteLine(result);

StringBuilder Create()
{
    var builder = new StringBuilder("log");
    return builder;
}
