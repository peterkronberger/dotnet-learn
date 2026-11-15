int calls = 0;

Func<string, bool> isLong = IsLong;
Console.WriteLine(calls);

Console.WriteLine(isLong("password"));
Console.WriteLine(isLong("abc"));
Console.WriteLine(calls);

bool IsLong(string text)
{
    calls++;
    return text.Length > 5;
}
