List<string> users = new List<string>();
users.Add("alice");
users.Add("bob");
users.Add("carol");

foreach (string user in users)
{
    if (user == "bob")
    {
        users.Remove(user);
    }
}
