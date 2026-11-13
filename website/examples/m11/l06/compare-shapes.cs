// The same two values in three shapes
var anonymous = new { Word = "fig", Length = 3 };
(string Word, int Length) tuple = ("fig", 3);
WordInfo record = new("fig", 3);

Console.WriteLine(anonymous);
Console.WriteLine(tuple);
Console.WriteLine(record);

Console.WriteLine(anonymous.Equals(new { Word = "fig", Length = 3 }));
Console.WriteLine(tuple.Equals(("fig", 3)));
Console.WriteLine(record.Equals(new WordInfo("fig", 3)));

// A tuple has mutable fields, an anonymous object has read-only properties
tuple.Length = 4;
Console.WriteLine(tuple);

record WordInfo(string Word, int Length);
