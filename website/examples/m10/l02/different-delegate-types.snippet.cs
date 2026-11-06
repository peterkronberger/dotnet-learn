NumberCheck check = IsEven;
Func<int, bool> same = check;

static bool IsEven(int number)
{
    return number % 2 == 0;
}

delegate bool NumberCheck(int number);
