namespace Tools
{
    static class TextExtensions
    {
        public static string Shout(this string text)
        {
            return text.ToUpper() + "!";
        }
    }
}

namespace App
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("hello".Shout());
        }
    }
}
