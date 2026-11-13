string[] left = ["a", "b", "c"];
string[] right = ["b", "c", "c", "d"];

var joined = left.GroupJoin(right, l => l, r => r, (l, rs) => $"{l}{rs.Count()}");
Console.WriteLine(string.Join(" ", joined));
