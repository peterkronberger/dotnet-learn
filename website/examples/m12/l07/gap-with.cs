// Change one property and keep the original
Ticket first = new(1, "open");
Ticket second = first with { Status = "closed" };

Console.WriteLine(first.Status);
Console.WriteLine(second.Status);

record Ticket(int Number, string Status);
