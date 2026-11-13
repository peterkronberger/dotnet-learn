string[] left = ["a", "b", "c"];
string[] right = ["b", "c", "c", "d"];

var joined = left.Join(right, l => l, r => r, (l, r) => l + r);
Console.WriteLine(string.Join(" ", joined));
