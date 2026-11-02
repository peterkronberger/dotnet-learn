Countdown(3);

void Countdown(int number)
{
    if (number == 0)
    {
        Console.WriteLine("Go");
    }
    else
    {
        Console.WriteLine(number);
        Countdown(number - 1);
    }
}
