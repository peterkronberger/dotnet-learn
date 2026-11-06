List<string> words = ["a", "b"];
IEnumerable<object> things = words;

int count = 0;
foreach (object thing in things)
{
    count++;
}
Console.WriteLine(count);
Console.WriteLine(things is IEnumerable<string>);
