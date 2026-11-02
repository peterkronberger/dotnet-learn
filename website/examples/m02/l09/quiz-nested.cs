int count = 0;
for (int a = 1; a <= 3; a++)
{
    for (int b = 1; b <= a; b++)
    {
        if (b == 2)
        {
            break;
        }
        count++;
    }
}
Console.WriteLine(count);
