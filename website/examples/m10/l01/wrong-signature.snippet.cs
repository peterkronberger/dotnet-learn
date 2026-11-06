NumberCheck check = Describe;

static string Describe(int number)
{
    return $"number {number}";
}

delegate bool NumberCheck(int number);
