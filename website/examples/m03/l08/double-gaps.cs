bool small = TryDouble(30, out int doubled);
Console.WriteLine($"{small} {doubled}");
small = TryDouble(80, out doubled);
Console.WriteLine($"{small} {doubled}");

bool TryDouble(int number, out int result)
{
    result = 0;
    return true;
}
