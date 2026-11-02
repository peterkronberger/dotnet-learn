int result = Square(5);
Console.WriteLine("Before the print");
Console.WriteLine(result);
Console.WriteLine(Square(3) + 1);
Square(4);
Console.WriteLine("End");

int Square(int number)
{
    return number * number;
}
