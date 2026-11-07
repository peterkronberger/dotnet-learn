Action<string>? notify = PrintPlain;
notify += PrintUpper;
notify += PrintPlain;

notify("disk");
Console.WriteLine(notify.GetInvocationList().Length);

notify -= PrintPlain;
notify?.Invoke("cpu");
Console.WriteLine(notify?.GetInvocationList().Length);

notify -= PrintPlain;
notify -= PrintUpper;
Console.WriteLine(notify is null);

static void PrintPlain(string text)
{
    Console.WriteLine(text);
}

static void PrintUpper(string text)
{
    Console.WriteLine(text.ToUpper());
}
