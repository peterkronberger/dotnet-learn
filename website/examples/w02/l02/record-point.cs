Alert first = new("ana", 3);
Alert second = new("ana", 3);

Console.WriteLine(first);
Console.WriteLine(first == second);
Console.WriteLine(first.User);

record Alert(string User, int Level);
