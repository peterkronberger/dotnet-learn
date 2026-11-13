List<int> numbers = [1, 2, 3];
IEnumerable<int> doubled = numbers.Select(n =>
{
    Console.WriteLine("select");
    return n * 2;
});
Console.WriteLine("defined");
foreach (int n in doubled)
{
    Console.WriteLine(n);
    break;
}
