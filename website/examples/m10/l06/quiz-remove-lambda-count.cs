Action? log = () => Console.WriteLine("a");
log += () => Console.WriteLine("b");
log -= () => Console.WriteLine("b");

Console.WriteLine(log?.GetInvocationList().Length);
