var first = new { Id = 1, Name = "a" };
var second = new { Id = 1, Name = "a" };
Console.WriteLine(first.Equals(second));
Console.WriteLine(ReferenceEquals(first, second));
