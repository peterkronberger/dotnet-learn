Console.WriteLine(Label(3));
Console.WriteLine(Word(1));

string Label(int count)
{
    return $"{count} {Word(count)}";

    string Word(int number)
    {
        return number == 1 ? "attempt" : "attempts";
    }
}
