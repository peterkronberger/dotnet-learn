int a = 1;
int b = 2;
Swap(ref a, ref b);
Console.WriteLine($"{a} {b}");

string first = "left";
string second = "right";
Swap<string>(ref first, ref second);
Console.WriteLine($"{first} {second}");

static void Swap<T>(ref T x, ref T y)
{
    T temp = x;
    x = y;
    y = temp;
}
