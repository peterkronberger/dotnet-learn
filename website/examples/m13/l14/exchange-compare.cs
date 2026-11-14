// Exchange and CompareExchange on one thread, to read what they return.
int state = 5;

int old = Interlocked.Exchange(ref state, 9);
Console.WriteLine($"Old: {old}, now: {state}");

int seen = Interlocked.CompareExchange(ref state, 12, 9);
Console.WriteLine($"Seen: {seen}, now: {state}");

seen = Interlocked.CompareExchange(ref state, 20, 9);
Console.WriteLine($"Seen: {seen}, now: {state}");
