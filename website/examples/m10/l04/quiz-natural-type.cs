var half = (double x) => x / 2;
var label = (string name, int count = 1) => $"{name} x{count}";

Console.WriteLine(half(5));
Console.WriteLine(label("rule"));
Console.WriteLine(label("rule", 3));
