string[] users = { "alice", "bob", "carol", "dave" };
Console.WriteLine(users[^1]);
Console.WriteLine(users[^2]);
Console.WriteLine(users[1..3].Length);
foreach (string user in users[1..3])
{
    Console.WriteLine(user);
}
Console.WriteLine(users[..2].Length);
Console.WriteLine(users[2..].Length);
