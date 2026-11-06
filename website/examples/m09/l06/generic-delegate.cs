Transformer<string, int> measure = Length;
Console.WriteLine(measure("hello"));

Transformer<int, string> format = Bracket;
Console.WriteLine(format(7));

Func<string, int> library = Length;
Console.WriteLine(library("abc"));

Action<string> show = Show;
show("done");

Predicate<int> isEven = IsEven;
Console.WriteLine(isEven(4));

static int Length(string text)
{
    return text.Length;
}

static string Bracket(int number)
{
    return $"[{number}]";
}

static void Show(string text)
{
    Console.WriteLine($"Show: {text}");
}

static bool IsEven(int number)
{
    return number % 2 == 0;
}

delegate TResult Transformer<TInput, TResult>(TInput input);
