// Positional patterns on a record and on a tuple
Visit[] visits =
[
    new("bob", "203.0.113.7"),
    new("dave", "203.0.113.7"),
    new("bob", "198.51.100.4"),
];
foreach (Visit visit in visits)
{
    bool fromServer = visit is (_, "203.0.113.7");
    bool isBob = visit is ("bob", var ip);
    Console.WriteLine($"{fromServer} {isBob}");
}

(string User, int Failures) pair = ("bob", 5);
Console.WriteLine(pair is ("bob", >= 3));
Console.WriteLine(pair is (not "bob", _));

record Visit(string User, string Ip);
