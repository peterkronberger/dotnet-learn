using System.Text;

var text = new StringBuilder("old");
Replace(text);
Console.WriteLine($"after value parameter: {text}");
ReplaceRef(ref text);
Console.WriteLine($"after ref parameter: {text}");

void Replace(StringBuilder builder)
{
    builder = new StringBuilder("new");
}

void ReplaceRef(ref StringBuilder builder)
{
    builder = new StringBuilder("new");
}
