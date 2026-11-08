int x = 5;
Reset(ref x);
Console.WriteLine(x);

void Reset(ref int number)
{
    number = 0;
}
