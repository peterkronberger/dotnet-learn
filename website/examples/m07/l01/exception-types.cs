int zero = 0;
int[] numbers = [1, 2, 3];
int index = 5;
object text = "seven";
string[] names = new string[1];
int big = int.MaxValue;

try
{
    Console.WriteLine(10 / zero);
}
catch (DivideByZeroException ex)
{
    Console.WriteLine($"Division: {ex.Message}");
}

try
{
    Console.WriteLine(numbers[index]);
}
catch (IndexOutOfRangeException ex)
{
    Console.WriteLine($"Index: {ex.Message}");
}

try
{
    Console.WriteLine((int)text);
}
catch (InvalidCastException ex)
{
    Console.WriteLine($"Cast: {ex.Message}");
}

try
{
    Console.WriteLine(names[0].Length);
}
catch (NullReferenceException ex)
{
    Console.WriteLine($"Null: {ex.Message}");
}

try
{
    Console.WriteLine(checked(big + 1));
}
catch (OverflowException ex)
{
    Console.WriteLine($"Overflow: {ex.Message}");
}
