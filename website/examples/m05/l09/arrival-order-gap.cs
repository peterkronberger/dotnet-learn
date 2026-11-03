string[] arrivals = ["alice", "bob", "carol"];
Stack<string> waiting = new Stack<string>();

foreach (string name in arrivals)
{
    waiting.Push(name);
}

while (waiting.Count > 0)
{
    Console.WriteLine(waiting.Pop());
}
