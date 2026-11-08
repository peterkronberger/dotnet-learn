Transformer<string, int> t = Length;
Console.WriteLine(t("hello"));

static int Length(string text)
{
    return text.Length;
}

delegate TResult Transformer<TInput, TResult>(TInput input);
