IProducer<string> words = new WordProducer();
IProducer<object> things = words;
Console.WriteLine(things.Next());

IConsumer<object> any = new PrintConsumer();
IConsumer<string> text = any;
text.Accept("hello");

interface IProducer<out T>
{
    T Next();
}

interface IConsumer<in T>
{
    void Accept(T item);
}

class WordProducer : IProducer<string>
{
    public string Next()
    {
        return "word";
    }
}

class PrintConsumer : IConsumer<object>
{
    public void Accept(object item)
    {
        Console.WriteLine($"Got: {item}");
    }
}
