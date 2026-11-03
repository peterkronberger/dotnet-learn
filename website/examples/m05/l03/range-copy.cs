int[] failures = { 1, 5, 0, 1 };
int[] part = failures[1..3];
part[0] = 99;
Console.WriteLine(failures[1]);
Console.WriteLine(part[0]);
string text = "login-failed";
Console.WriteLine(text[6..]);
Console.WriteLine(text[..5]);
