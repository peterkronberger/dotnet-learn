Console.WriteLine("hello".Shout());
Console.WriteLine(TextExtensions.Shout("hello"));
Console.WriteLine("hello".Wrap("*"));

static class TextExtensions
{
    public static string Shout(this string text)
    {
        return text.ToUpper() + "!";
    }

    public static string Wrap(this string text, string mark)
    {
        return mark + text + mark;
    }
}
