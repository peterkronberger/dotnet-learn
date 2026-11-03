PrintLength("alice");
PrintLength(null);

void PrintLength(string? text)
{
    if (text != null)
    {
        Console.WriteLine($"Length: {text.Length}");
    }
}
