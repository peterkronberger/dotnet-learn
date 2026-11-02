string text = "12x45";
int withBreak = 0;
foreach (char symbol in text)
{
    if (symbol == 'x')
    {
        break;
    }
    withBreak++;
}
int withContinue = 0;
foreach (char symbol in text)
{
    if (symbol == 'x')
    {
        continue;
    }
    withContinue++;
}
Console.WriteLine($"{withBreak} {withContinue}");
