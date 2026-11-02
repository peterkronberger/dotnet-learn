var original = (User: "bob", Failures: 5);
var copy = original;
copy.Failures = 6;
Console.WriteLine($"{original.Failures} {copy.Failures}");
