string folder = Directory.CreateTempSubdirectory("seclog-").FullName;
try
{
    string missing = Path.Combine(folder, "missing.txt");
    try
    {
        File.ReadAllText(missing);
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine($"1: {ex.GetType().Name}");
    }

    string inMissingFolder = Path.Combine(folder, "none", "x.txt");
    try
    {
        File.WriteAllText(inMissingFolder, "x");
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine($"2: {ex.GetType().Name}");
    }

    try
    {
        File.ReadAllText(inMissingFolder);
    }
    catch (IOException ex)
    {
        Console.WriteLine($"3: {ex.GetType().Name}");
    }
}
finally
{
    Directory.Delete(folder, true);
}
