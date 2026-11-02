int total = 14;
Console.WriteLine(Describe(in total));
Console.WriteLine(Describe(7));

string Describe(in int events)
{
    return $"{events} events";
}
