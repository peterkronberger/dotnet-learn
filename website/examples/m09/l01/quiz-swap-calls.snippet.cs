int a = 1;
int b = 2;
string s = "s";

static void Swap<T>(ref T x, ref T y)
{
    T temp = x;
    x = y;
    y = temp;
}
