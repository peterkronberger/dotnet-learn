using System.Text;

var dir = Directory.CreateTempSubdirectory("seclog-");
Console.WriteLine("CreateTempSubdirectory: " + dir.Exists);

try
{
    var path = Path.Combine(dir.FullName, "notes.txt");
    Console.WriteLine("Path.Combine: " + Path.GetFileName(path));
    Console.WriteLine("GetFileName: " + Path.GetFileName(path));
    Console.WriteLine("GetExtension: " + Path.GetExtension(path));

    File.WriteAllText(path, "first");
    Console.WriteLine("WriteAllText/ReadAllText: " + File.ReadAllText(path));

    File.AppendAllText(path, " second");
    Console.WriteLine("AppendAllText: " + File.ReadAllText(path));

    var linesPath = Path.Combine(dir.FullName, "lines.txt");
    File.WriteAllLines(linesPath, ["a", "b", "c"]);
    var all = File.ReadAllLines(linesPath);
    Console.WriteLine("WriteAllLines/ReadAllLines: " + all.Length);

    var count = 0;
    foreach (var line in File.ReadLines(linesPath))
    {
        count++;
    }
    Console.WriteLine("ReadLines: " + count);

    var writerPath = Path.Combine(dir.FullName, "writer.txt");
    using (var writer = new StreamWriter(writerPath))
    {
        writer.WriteLine("one");
        writer.WriteLine("two");
    }
    Console.WriteLine("StreamWriter.WriteLine: done");

    using (var reader = new StreamReader(writerPath))
    {
        var read = 0;
        string? text = reader.ReadLine();
        while (text != null)
        {
            read++;
            text = reader.ReadLine();
        }
        Console.WriteLine("StreamReader.ReadLine: " + read);
    }

    var utf8Path = Path.Combine(dir.FullName, "utf8.txt");
    File.WriteAllText(utf8Path, "plain ascii", Encoding.UTF8);
    Console.WriteLine("Encoding.UTF8: " + File.ReadAllText(utf8Path));

    Console.WriteLine("Exists before Delete: " + File.Exists(path));
    File.Delete(path);
    Console.WriteLine("Exists after Delete: " + File.Exists(path));

    try
    {
        File.ReadAllText(path);
    }
    catch (FileNotFoundException ex)
    {
        Console.WriteLine("Missing file: " + ex.GetType().Name);
    }

    var badPath = Path.Combine(dir.FullName, "no-such-dir", "x.txt");
    try
    {
        File.WriteAllText(badPath, "x");
    }
    catch (DirectoryNotFoundException ex)
    {
        Console.WriteLine("Missing directory: " + ex.GetType().Name);
    }

    try
    {
        File.ReadAllText(path);
    }
    catch (IOException)
    {
        Console.WriteLine("catch (IOException): file");
    }
    try
    {
        File.WriteAllText(badPath, "x");
    }
    catch (IOException)
    {
        Console.WriteLine("catch (IOException): directory");
    }
}
finally
{
    Directory.Delete(dir.FullName, true);
    Console.WriteLine("Directory.Delete: " + !Directory.Exists(dir.FullName));
}
