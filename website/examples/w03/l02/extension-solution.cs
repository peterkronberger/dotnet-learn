Console.WriteLine("ana".Length);
Console.WriteLine("ana".Shout());

static class TextExtensions
{
    public static string Shout(this string text)
    {
        return text.ToUpper() + "!";
    }
}
