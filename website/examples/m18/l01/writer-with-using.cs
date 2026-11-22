DirectoryInfo folder = Directory.CreateTempSubdirectory();
string path = Path.Combine(folder.FullName, "report.txt");

using (StreamWriter writer = new(path))
{
    writer.Write("bob: 5 failures");
}
Console.WriteLine($"Bytes in the file: {new FileInfo(path).Length}");

folder.Delete(recursive: true);
