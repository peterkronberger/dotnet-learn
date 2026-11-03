string[] users = { "alice", "bob", "carol", "dave" };
// The range should select the last two users
foreach (string user in users[2..3])
{
    Console.WriteLine(user);
}
