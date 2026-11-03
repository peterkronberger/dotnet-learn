int number = 7;
object first = number;
object second = number;
Console.WriteLine(first.Equals(second));
Console.WriteLine(object.ReferenceEquals(first, second));
Console.WriteLine(first == second);
