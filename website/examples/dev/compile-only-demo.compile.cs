// Compile-only example: the output depends on timing, so CI never runs it.
var first = Task.Run(() => Console.WriteLine("A"));
var second = Task.Run(() => Console.WriteLine("B"));
await Task.WhenAll(first, second);