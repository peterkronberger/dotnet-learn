string[] users = ["bob", "alice", "bob", "dave", "alice", "bob"];

var best = users
    .GroupBy(u => u)
    .OrderByDescending(g => g.Count())
    .Take(1);

foreach (var userGroup in best)
{
    Console.WriteLine($"{userGroup.Key} {userGroup.Count()}");
}
