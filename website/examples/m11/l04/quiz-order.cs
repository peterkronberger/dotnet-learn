static IEnumerable<char> Letters()
{
    Console.WriteLine("a");
    yield return 'x';
    Console.WriteLine("b");
    yield return 'y';
}

foreach (char letter in Letters())
{
    Console.WriteLine(letter);
}
