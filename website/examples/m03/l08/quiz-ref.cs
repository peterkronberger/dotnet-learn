int first = 1;
int second = 2;
Swap(ref first, ref second);
Console.WriteLine($"{first} {second}");

void Swap(ref int left, ref int right)
{
    int saved = left;
    left = right;
    right = saved;
}
