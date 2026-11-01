int count = 0;
bool a = true;
bool b = a || (++count > 0);
bool c = !a & (++count > 0);
Console.WriteLine($"{b} {c} {count}");
