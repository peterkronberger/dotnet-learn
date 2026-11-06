using System.Collections;

ArrayList old = new ArrayList();
old.Add(42);
old.Add("text");
int first = (int)old[0]!;
Console.WriteLine($"First: {first}");
try
{
    int second = (int)old[1]!;
    Console.WriteLine(second);
}
catch (InvalidCastException ex)
{
    Console.WriteLine(ex.GetType().Name);
}

List<int> numbers = [42, 7];
int firstNumber = numbers[0];
Console.WriteLine($"First: {firstNumber}");
