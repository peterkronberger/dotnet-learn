Action<string> show = text => Console.WriteLine($"1 {text}");
show += text => Console.WriteLine($"2 {text}");
Action<string> early = text => Console.WriteLine($"0 {text}");
early += show;

early("go");
Console.WriteLine(early.GetInvocationList().Length);
