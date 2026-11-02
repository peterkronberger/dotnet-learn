int sum = 0;
for (int i = 1; i <= 6; i++)
{
    if (i == 2)
    {
        continue;
    }
    if (i == 5)
    {
        break;
    }
    sum += i;
}
Console.WriteLine(sum);
