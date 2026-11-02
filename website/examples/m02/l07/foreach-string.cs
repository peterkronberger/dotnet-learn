string word = "banana";
int count = 0;
foreach (char letter in word)
{
    if (letter == 'a')
    {
        count++;
    }
}
Console.WriteLine($"{count} times a");
