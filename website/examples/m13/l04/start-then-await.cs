// Start both tasks first, then wait for both
List<string> steps = new();
Task a = WorkAsync("A", steps);
Task b = WorkAsync("B", steps);
steps.Add("both started");
await Task.WhenAll(a, b);
steps.Add("all done");
foreach (string step in steps)
{
    Console.WriteLine(step);
}

async Task WorkAsync(string name, List<string> log)
{
    log.Add($"start {name}");
    await Task.Delay(10);
}
