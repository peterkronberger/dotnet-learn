DirectoryInfo folder = Directory.CreateTempSubdirectory();
string path = Path.Combine(folder.FullName, "report.txt");

StreamWriter writer = new(path);
writer.Write("bob: 5 failures");
Console.WriteLine($"Bytes in the file: {new FileInfo(path).Length}");

writer.Dispose();
Console.WriteLine($"After Dispose: {new FileInfo(path).Length}");

folder.Delete(recursive: true);
