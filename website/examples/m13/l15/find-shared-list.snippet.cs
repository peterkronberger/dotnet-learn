// Two tasks collect the failed users into one list.
List<string> failedUsers = [];

Task first = Task.Run(() => Collect(["bob", "alice"]));
Task second = Task.Run(() => Collect(["dave"]));
await Task.WhenAll(first, second);
Console.WriteLine(failedUsers.Count);

void Collect(string[] users)
{
    foreach (string user in users)
    {
        failedUsers.Add(user);
    }
}
