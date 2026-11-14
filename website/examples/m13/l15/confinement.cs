// Each worker counts in its own variable; the results are merged at the end.
string[][] parts =
[
    ["login-failed", "login-success", "login-failed"],
    ["logout", "login-failed"],
];

List<Task<int>> workers = [];
foreach (string[] part in parts)
{
    workers.Add(Task.Run(() => part.Count(e => e == "login-failed")));
}

int[] counts = await Task.WhenAll(workers);
Console.WriteLine($"Per worker: {string.Join(", ", counts)}");
Console.WriteLine($"Total: {counts.Sum()}");
