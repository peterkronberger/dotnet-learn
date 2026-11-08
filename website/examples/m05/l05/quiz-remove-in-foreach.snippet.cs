var list = new List<string>();
list.Add("alice");
list.Add("bob");
list.Add("carol");

foreach (string user in list)
{
    if (user == "bob")
    {
        list.Remove(user);
    }
}
