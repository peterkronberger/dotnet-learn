string folder = Directory.CreateTempSubdirectory("seclog-").FullName;
try
{
    string path = Path.Combine(folder, "events.txt");
    using (StreamWriter writer = new(path))
    {
        writer.WriteLine("login-failed");
        writer.WriteLine("logout");
        writer.WriteLine("login-failed");
    }

    int failed = 0;
    using (StreamReader reader = new(path))
    {
        string? line = reader.ReadLine();
        while (line != null)
        {
            Console.WriteLine($"Read: {line}");
            if (line == "login-failed")
            {
                failed++;
            }
            line = reader.ReadLine();
        }
    }
    Console.WriteLine($"Failed: {failed}");
}
finally
{
    Directory.Delete(folder, true);
}
