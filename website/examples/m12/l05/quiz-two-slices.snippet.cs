string[] parts = ["a", "b", "c"];
Console.WriteLine(parts is ["a", ..]);
Console.WriteLine(parts is [.., "b", ..]);
Console.WriteLine(parts is [.., "c"]);
