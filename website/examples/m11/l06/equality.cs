var a = new { User = "bob", Hour = 8 };
var b = new { User = "bob", Hour = 8 };
var c = new { User = "bob", Hour = 9 };
var d = new { Hour = 8, User = "bob" };

Console.WriteLine(a.Equals(b));
Console.WriteLine(a.Equals(c));
Console.WriteLine(a.Equals(d));
Console.WriteLine(ReferenceEquals(a, b));
Console.WriteLine(a.GetHashCode() == b.GetHashCode());

// The same shape gives the same type, so a can get the value of b
a = b;
Console.WriteLine(a);
