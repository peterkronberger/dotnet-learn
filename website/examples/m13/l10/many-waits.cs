// 1000 tasks wait at the same time. An await needs no thread while it waits.
Task[] waits = new Task[1000];
for (int i = 0; i < waits.Length; i++)
{
    waits[i] = Task.Delay(20);
}

await Task.WhenAll(waits);
Console.WriteLine($"All {waits.Length} waits finished");
