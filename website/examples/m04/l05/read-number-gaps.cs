Show(ReadNumber("42"));
Show(ReadNumber("many"));

int? ReadNumber(string text)
{
    if (int.TryParse(text, out int number))
    {
        return number;
    }
    return 0;
}

void Show(int? value)
{
    Console.WriteLine(value.HasValue ? $"Number: {value.Value}" : "No number");
}
