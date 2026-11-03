int number = 42;
object boxed = number;
number = 43;
Console.WriteLine(number);
Console.WriteLine(boxed);
int back = (int)boxed;
Console.WriteLine(back);
