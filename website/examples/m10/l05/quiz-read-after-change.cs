int x = 1;
Func<int> read = () => x;
x = 5;
Console.WriteLine(read());
