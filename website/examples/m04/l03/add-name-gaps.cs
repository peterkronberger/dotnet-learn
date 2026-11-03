using System.Text;

var names = new StringBuilder();
Add(names, "alice");
Add(names, "bob");
Console.WriteLine(names);

void Add(StringBuilder target, string name)
{
    target = new StringBuilder(name);
}
