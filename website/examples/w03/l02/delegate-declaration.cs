static bool IsShort(string text)
{
    return text.Length < 6;
}

Check isShort = IsShort;
Console.WriteLine(isShort("abc"));
Console.WriteLine(isShort("passphrase"));

delegate bool Check(string text);
