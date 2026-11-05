string text = Join("a", "b");
Console.WriteLine(text);

string Join(string first, string second)
{
    // The using declaration disposes the writer at the end of the method
    using StringWriter writer = new();
    writer.Write(first);
    writer.Write("+");
    writer.Write(second);
    return writer.ToString();
}
