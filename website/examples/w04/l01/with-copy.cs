Incident first = new("INC-1", "open");
Incident second = first with { State = "closed" };
Console.WriteLine($"first: {first.State}");
Console.WriteLine($"second: {second.State}");
Console.WriteLine($"same Id: {first.Id == second.Id}");

record Incident(string Id, string State);
