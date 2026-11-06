// An optional parameter: the default value is used if the argument is missing
var log = (string text, string level = "INFO") => $"[{level}] {text}";

// A parameter array: any number of arguments
var count = (params int[] values) => values.Length;

Console.WriteLine(log("started"));
Console.WriteLine(log("disk full", "WARN"));
Console.WriteLine(count());
Console.WriteLine(count(4, 8, 15));
