string[] empty = [];
string[] one = ["a"];
string[] two = ["a", "b"];
Console.WriteLine(empty is [..]);
Console.WriteLine(one is [_, ..]);
Console.WriteLine(one is [_, _, ..]);
Console.WriteLine(two is [.., "b"]);
