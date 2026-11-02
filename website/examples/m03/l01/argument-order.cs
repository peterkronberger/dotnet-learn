int n = 0;
Console.WriteLine(Pair(n++, n++));
Console.WriteLine(n);

string Pair(int first, int second)
{
    return $"first={first}, second={second}";
}
