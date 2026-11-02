Show(3);

void Show(int number)
{
    Console.WriteLine($"enter {number}");
    if (number > 1)
    {
        Show(number - 1);
    }
    Console.WriteLine($"leave {number}");
}
