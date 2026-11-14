// Task.WhenEach hands out the tasks in the order they finish
Task<string>[] tasks =
[
    NameAfterAsync("slow", 300),
    NameAfterAsync("fast", 10),
    NameAfterAsync("middle", 100),
];

await foreach (Task<string> finished in Task.WhenEach(tasks))
{
    Console.WriteLine($"Finished: {await finished}");
}

async Task<string> NameAfterAsync(string name, int milliseconds)
{
    await Task.Delay(milliseconds);
    return name;
}
