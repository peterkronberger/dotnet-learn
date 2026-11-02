Console.WriteLine(Info("a"));
Console.WriteLine(Info("b", level: 2));
Console.WriteLine(Info(level: 3, text: "c"));

string Info(string text, int level = 1) => $"{level}:{text}";
