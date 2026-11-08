Console.WriteLine(Label(3));

string Label(int count)
{
    string text = Word(count);
    return text;

    string Word(int number)
    {
        return number == 1 ? "attempt" : "attempts";
    }
}
