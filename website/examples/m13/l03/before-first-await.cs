// An error before the first await is stored in the task as well
Task<int> task = FailAtOnceAsync();
Console.WriteLine($"Call returned, faulted: {task.IsFaulted}");
try
{
    await task;
}
catch (ArgumentException ex)
{
    Console.WriteLine($"Caught {ex.GetType().Name}");
}

async Task<int> FailAtOnceAsync()
{
    await Task.CompletedTask;
    throw new ArgumentException("Bad input");
}
