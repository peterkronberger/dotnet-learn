Console.WriteLine(Price(10));

int Price(int net)
{
    int tax = 2;
    return AddTax();

    static int AddTax()
    {
        return net + tax;
    }
}
