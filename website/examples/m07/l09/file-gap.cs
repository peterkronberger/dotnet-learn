string folder = Directory.CreateTempSubdirectory("seclog-").FullName;
try
{
    string path = Path.Combine(folder, "events.txt");
    File.WriteAllLines(path, ["login-failed", "logout", "login-failed"]);

    int failed = 0;
    foreach (string line in File.ReadAllLines(path))
    {
        if (line == "login-failed")
        {
            failed++;
        }
    }
    Console.WriteLine($"Failed: {failed}");
}
finally
{
    Directory.Delete(folder, true);
}
