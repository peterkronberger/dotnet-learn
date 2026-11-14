// Each iteration writes only its own array slot, so no lock is needed.
string[] lines =
[
    "login-failed user=bob",
    "login-success user=alice",
    "login-failed user=dave",
    "logout user=carol",
];

bool[] isFailure = new bool[lines.Length];
Parallel.For(0, lines.Length, i =>
{
    isFailure[i] = lines[i].Contains("login-failed");
});

Console.WriteLine($"Flags: {string.Join(", ", isFailure)}");
Console.WriteLine($"Failures: {isFailure.Count(f => f)}");
