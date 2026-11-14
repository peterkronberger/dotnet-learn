// One task per file, without any limit: with 10,000 files, 10,000 reads start at once
List<Task<int>> tasks = [];
foreach (string path in Directory.GetFiles(logDirectory))
{
    tasks.Add(CountFailuresInFileAsync(path));
}
int[] counts = await Task.WhenAll(tasks);
