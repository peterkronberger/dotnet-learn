// One worker thread computes a result. The main thread prints it only after Join.
int result = 0;
Thread worker = new(() => result = 1 + 2);
worker.Start();
worker.Join();
Console.WriteLine($"Result: {result}");
