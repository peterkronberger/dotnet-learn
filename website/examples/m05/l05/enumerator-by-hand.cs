List<string> users = new List<string>();
users.Add("alice");
users.Add("bob");

var enumerator = users.GetEnumerator();
while (enumerator.MoveNext())
{
    Console.WriteLine(enumerator.Current);
}

foreach (string user in users)
{
    Console.WriteLine(user);
}
