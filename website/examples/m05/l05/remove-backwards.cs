List<string> users = new List<string>();
users.Add("bob");
users.Add("alice");
users.Add("bob");
users.Add("carol");

for (int i = users.Count - 1; i >= 0; i--)
{
    if (users[i] == "bob")
    {
        users.RemoveAt(i);
    }
}

Console.WriteLine(users.Count);
Console.WriteLine(users[0]);
Console.WriteLine(users[1]);
