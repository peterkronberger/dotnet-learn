Console.WriteLine(Describe(0));
Console.WriteLine(Describe(7));

string Describe(int count)
{
    if (count == 0)
    {
        return "none";
    }
    Console.WriteLine("count is not zero");
    return "some";
}
