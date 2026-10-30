// The compiler takes the type from the value on the right
var count = 7;
var divisor = 2;
var ratio = 7.0;
Console.WriteLine(count / divisor);
Console.WriteLine(ratio / divisor);
// The type stays fixed: count is an int, so this is still an integer division
count = count / 2;
Console.WriteLine(count);
