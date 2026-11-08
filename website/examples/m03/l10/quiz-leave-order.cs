Show(3);

void Show(int n)
{
    Console.WriteLine($"enter {n}");
    if (n > 1)
    {
        Show(n - 1);
    }
    Console.WriteLine($"leave {n}");
}
