string folder = Directory.CreateTempSubdirectory("seclog-").FullName;
try
{
    string path = Path.Combine(folder, "notes.txt");
    Console.WriteLine($"Name: {Path.GetFileName(path)}");
    Console.WriteLine($"Extension: {Path.GetExtension(path)}");
    Console.WriteLine($"Exists before: {File.Exists(path)}");

    File.WriteAllLines(path, ["first", "second"]);
    File.AppendAllText(path, "third\n");
    Console.WriteLine($"Exists after: {File.Exists(path)}");

    string[] lines = File.ReadAllLines(path);
    Console.WriteLine($"Lines: {lines.Length}");
    foreach (string line in File.ReadLines(path))
    {
        Console.WriteLine($"- {line}");
    }
}
finally
{
    Directory.Delete(folder, true);
}
