// Named members: the name is written before the =
var row = new { Name = "bob", Hour = 8 };
Console.WriteLine($"{row.Name} at {row.Hour}");

// Inferred names: a simple name or a member access gives its own name
string word = "fig";
var info = new { word, word.Length };
Console.WriteLine($"{info.word} has {info.Length} letters");

// The type has no name, so the variable needs var
Console.WriteLine(row);
Console.WriteLine(info);
