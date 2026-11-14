Visit[] visits =
[
    new("bob", "203.0.113.7"),
    new("203.0.113.7", "bob"),
    new("dave", "bob"),
];
foreach (Visit visit in visits)
{
    Console.WriteLine(visit is (_, "bob"));
}

record Visit(string User, string Ip);
