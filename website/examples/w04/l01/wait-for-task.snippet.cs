// Two ways to wait for the same task
int LoadBlocking()
{
    Task<int> task = CountAsync();
    return task.Result;
}

async Task<int> LoadAsync()
{
    Task<int> task = CountAsync();
    return await task;
}
