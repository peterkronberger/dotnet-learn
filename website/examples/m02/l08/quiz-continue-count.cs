int n = 0;
foreach (char c in "12x45")
{
    if (c == 'x')
    {
        continue;
    }
    n++;
}
Console.WriteLine(n);
