string[] users = ["ana", "ben", "cem", "dia", "eli"];

string[] part = users[1..3];
Console.WriteLine(part.Length);
Console.WriteLine(string.Join(",", part));
Console.WriteLine(users[^1]);
