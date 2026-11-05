string folder = Directory.CreateTempSubdirectory("seclog-").FullName;
try
{
    string path = Path.Combine(folder, "note.txt");
    File.WriteAllText(path, "one");
    File.WriteAllText(path, "two");
    Console.WriteLine($"After two WriteAllText: {File.ReadAllText(path)}");

    File.AppendAllText(path, "-three");
    Console.WriteLine($"After AppendAllText: {File.ReadAllText(path)}");
}
finally
{
    Directory.Delete(folder, true);
}
