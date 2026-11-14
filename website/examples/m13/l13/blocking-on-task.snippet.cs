// Part of an app with a window. Its context runs code on one thread only.
void OnRefreshClicked()
{
    // Blocks the context thread until the task is done.
    string[] lines = LoadSourceAsync("web").Result;
    Console.WriteLine(lines.Length);
}

async Task<string[]> LoadSourceAsync(string name)
{
    await Task.Delay(100);
    // The rest wants to continue on the context thread. It is blocked.
    return [];
}
