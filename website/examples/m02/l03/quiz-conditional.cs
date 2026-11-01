int a = 0;
int b = 0;
bool flag = false;
int r = flag ? ++a : ++b + 10;
Console.WriteLine($"{r} {a} {b}");
