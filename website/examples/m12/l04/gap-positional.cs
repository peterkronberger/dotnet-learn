// Count the visits from one server with a positional pattern
Visit[] visits =
[
    new("bob", "203.0.113.7"),
    new("dave", "203.0.113.7"),
    new("bob", "203.0.113.7"),
    new("bob", "198.51.100.4"),
];
int count = 0;
foreach (Visit visit in visits)
{
    if (visit is (_, "203.0.113.7"))
    {
        count++;
    }
}
Console.WriteLine(count);

var (name, _) = visits[3];
Console.WriteLine(name);

record Visit(string User, string Ip);
