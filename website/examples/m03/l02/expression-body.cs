Console.WriteLine(SquareBlock(6));
Console.WriteLine(SquareShort(6));
Console.WriteLine(IsEven(6));
Console.WriteLine(IsEven(7));

int SquareBlock(int number)
{
    return number * number;
}

int SquareShort(int number) => number * number;

bool IsEven(int number) => number % 2 == 0;
