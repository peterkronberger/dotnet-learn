List<string> users = new List<string>();
users.Add("bob");
users.Add("bob");
users.Add("alice");

for (int i = 0; i < users.Count; i++)
{
    if (users[i] == "bob")
    {
        users.RemoveAt(i);
    }
}

Console.WriteLine(users.Count);
Console.WriteLine(users[0]);
