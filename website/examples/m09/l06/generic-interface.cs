IConverter<string, int> measure = new LengthConverter();
Console.WriteLine(measure.Convert("hello"));

IConverter<int, string> number = new Bracketer<int>();
Console.WriteLine(number.Convert(42));

Bracketer<bool> flag = new Bracketer<bool>();
Console.WriteLine(flag.Convert(true));

interface IConverter<TInput, TOutput>
{
    TOutput Convert(TInput input);
}

class LengthConverter : IConverter<string, int>
{
    public int Convert(string input)
    {
        return input.Length;
    }
}

class Bracketer<T> : IConverter<T, string>
{
    public string Convert(T input)
    {
        return $"[{input}]";
    }
}
