Func<int, string> label = number =>
{
    if (number < 0)
    {
        return "negative";
    }
    return number == 0 ? "zero" : "positive";
};

Console.WriteLine(label(-3));
Console.WriteLine(label(0));
Console.WriteLine(label(5));
