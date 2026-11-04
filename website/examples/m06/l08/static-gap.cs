Calculator calculator = new Calculator();
Console.WriteLine(calculator.Double(4));
Console.WriteLine(calculator.Double(5));

class Calculator
{
    public int Double(int value)
    {
        return value * 2;
    }
}
