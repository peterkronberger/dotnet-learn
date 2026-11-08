Countdown(3);

void Countdown(int number)
{
    Console.WriteLine(number);
    Countdown(number - 1);
}
