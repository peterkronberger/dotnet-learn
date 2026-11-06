object first = Pick(true, 10, 20);
int number = (int)first;
Console.WriteLine(number + 1);

object word = Pick(false, "ten", "twenty");
try
{
    int wrong = (int)word;
    Console.WriteLine(wrong);
}
catch (InvalidCastException ex)
{
    Console.WriteLine(ex.GetType().Name);
}

static object Pick(bool takeFirst, object a, object b)
{
    if (takeFirst)
    {
        return a;
    }
    return b;
}
