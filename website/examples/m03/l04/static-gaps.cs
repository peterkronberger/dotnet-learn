Console.WriteLine(Price(10));
Console.WriteLine(Price(20));

int Price(int net)
{
    int tax = 2;
    return AddTax();

    int AddTax()
    {
        return net + tax;
    }
}
