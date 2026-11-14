// Fill the gaps: handle the error of the task
Task<string> task = ReadNameAsync();
try
{
    string name = await task;
    Console.WriteLine($"Name: {name}");
}
catch (FormatException ex)
{
    Console.WriteLine($"Failed: {ex.GetType().Name}");
}
Console.WriteLine($"Faulted: {task.IsFaulted}");

async Task<string> ReadNameAsync()
{
    await Task.Delay(10);
    throw new FormatException("Name is not valid");
}
