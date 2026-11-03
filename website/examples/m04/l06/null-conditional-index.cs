string? user = "bob";
string? none = null;
Console.WriteLine(user[0]);
Console.WriteLine(user?[0]);
char? first = none?[0];
Console.WriteLine(first == null);
