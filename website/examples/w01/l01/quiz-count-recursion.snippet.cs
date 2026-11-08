Console.WriteLine(Count(3));

int Count(int n)
{
    return Count(n - 1) + 1;
}
