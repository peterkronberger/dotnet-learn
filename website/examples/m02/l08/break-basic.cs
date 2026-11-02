foreach (char letter in "ab#cd")
{
    if (letter == '#')
    {
        break;
    }
    Console.WriteLine(letter);
}
Console.WriteLine("stopped");
