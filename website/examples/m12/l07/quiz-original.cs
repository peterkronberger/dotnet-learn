Ticket a = new(7, "open");
Ticket b = a with { Number = 8 };
Ticket c = b with { Status = "closed" };

Console.WriteLine(a);
Console.WriteLine(b);
Console.WriteLine(c);

record Ticket(int Number, string Status);
