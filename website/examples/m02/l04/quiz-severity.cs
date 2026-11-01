int value = 5;
string text = value switch
{
    > 5 => "A",
    >= 5 => "B",
    _ => "C",
};
Console.WriteLine(text);
