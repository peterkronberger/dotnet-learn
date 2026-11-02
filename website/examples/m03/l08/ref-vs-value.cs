int attempts = 1;
AddOneToCopy(attempts);
Console.WriteLine(attempts);
AddOneToOriginal(ref attempts);
Console.WriteLine(attempts);

void AddOneToCopy(int number)
{
    number++;
}

void AddOneToOriginal(ref int number)
{
    number++;
}
