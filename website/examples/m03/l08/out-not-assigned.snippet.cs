// This fragment does not compile
bool TryHalf(int number, out int half)
{
    if (number % 2 == 0)
    {
        half = number / 2;
        return true;
    }
    return false;
}
