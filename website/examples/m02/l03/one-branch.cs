int yes = 0;
int no = 0;
bool ok = true;
int result = ok ? ++yes : ++no;
Console.WriteLine($"result: {result}, yes: {yes}, no: {no}");
