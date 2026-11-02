var result = MinMax(7, 2, 9);
Console.WriteLine($"{result.Smallest} {result.Largest}");

var (low, high) = MinMax(4, 8, 6);
Console.WriteLine($"{low} {high}");

int first;
int second;
(first, second) = MinMax(5, 5, 1);
Console.WriteLine($"{first} {second}");

(int Smallest, int Largest) MinMax(int a, int b, int c)
{
    int smallest = Math.Min(a, Math.Min(b, c));
    int largest = Math.Max(a, Math.Max(b, c));
    return (smallest, largest);
}
