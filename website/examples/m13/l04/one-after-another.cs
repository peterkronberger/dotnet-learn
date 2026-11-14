// Two awaits in a row: B starts only after A is done
List<string> steps = new();
await WorkAsync("A", steps);
steps.Add("A done");
await WorkAsync("B", steps);
steps.Add("B done");
foreach (string step in steps)
{
    Console.WriteLine(step);
}

async Task WorkAsync(string name, List<string> log)
{
    log.Add($"start {name}");
    await Task.Delay(10);
}
