Action log = () => Console.WriteLine("a");
log += () => Console.WriteLine("b");
log -= () => Console.WriteLine("b");
Console.WriteLine(log.GetInvocationList().Length);

Action second = () => Console.WriteLine("c");
log += second;
log -= second;
Console.WriteLine(log.GetInvocationList().Length);
