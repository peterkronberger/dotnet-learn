const int Factor = 2;
int offset = 10;

Func<int, int> ok = static number => number * Factor;
Func<int, int> broken = static number => number + offset;
