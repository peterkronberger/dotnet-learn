Console.WriteLine(Label(1));
Console.WriteLine(Label(3));

string Label(int count)
{
    string word = Word(count);
    return $"{count} {word}";

    string Word(int number)
    {
        return number == 1 ? "attempt" : "attempts";
    }
}
