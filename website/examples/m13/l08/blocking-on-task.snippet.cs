// Three ways to block a thread until a task is done. Do not use them.
int first = CountAsync().Result;
CountAsync().Wait();
int second = CountAsync().GetAwaiter().GetResult();

// Wait with await instead
int third = await CountAsync();

async Task<int> CountAsync()
{
    await Task.Delay(10);
    return 7;
}
