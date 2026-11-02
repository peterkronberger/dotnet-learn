Console.WriteLine(Twice(4));

int Twice(int number)
{
    return Double(number) + Double(number);

    int Double(int value)
    {
        return value * 2;
    }
}
