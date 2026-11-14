// Library code: do not return to the captured synchronization context
public async Task<int> CountLinesAsync(string path)
{
    string[] lines = await File.ReadAllLinesAsync(path).ConfigureAwait(false);
    return lines.Length;
}
