int n = 3;
string text = n switch { 1 => "one", 3 => "three", _ => "other" };
Console.WriteLine(text);
