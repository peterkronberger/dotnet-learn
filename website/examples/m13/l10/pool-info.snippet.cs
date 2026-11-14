// The output depends on the machine and on the run.
Console.WriteLine(Environment.ProcessorCount);
Console.WriteLine(Environment.CurrentManagedThreadId);

await Task.Run(() => Console.WriteLine(Environment.CurrentManagedThreadId));
