bool stop = false;
int checks = 0;
// 1. && skips the right side when the left side is false
bool a = stop && (++checks > 0);
Console.WriteLine($"a={a}, checks={checks}");
// 2. & always evaluates both sides
bool b = stop & (++checks > 0);
Console.WriteLine($"b={b}, checks={checks}");
// 3. || skips the right side when the left side is true
bool c = !stop || (++checks > 0);
Console.WriteLine($"c={c}, checks={checks}");
