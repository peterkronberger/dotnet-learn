// Create a changed copy of a record with a with expression
Ticket first = new(1, "open");
Ticket second = first with { Status = "closed" };

Console.WriteLine(first);
Console.WriteLine(second);

// An empty with expression copies without changing anything
Ticket copy = first with { };
Console.WriteLine(first == copy);
Console.WriteLine(ReferenceEquals(first, copy));

record Ticket(int Number, string Status);
