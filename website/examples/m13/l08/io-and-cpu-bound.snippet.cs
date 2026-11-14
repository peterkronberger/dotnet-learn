// I/O-bound: waiting for the outside world. Await the async API.
string[] lines = await File.ReadAllLinesAsync(path);

// CPU-bound: computing. Task.Run moves the work to a thread pool thread.
int total = await Task.Run(() => CountPrimes(2_000_000));
