string[] lines =
{
    "alice login-failure",
    "alice login-failure 192.0.2.10",
    "login-failure"
};
foreach (string line in lines)
{
    string[] parts = line.Split(' ');
    bool exact = parts is [_, "login-failure"];
    bool prefix = parts is [_, "login-failure", ..];
    Console.WriteLine($"{line}: {exact} {prefix}");
}
