// Before C# 14: Doubler twiceOld = (ref int x) => x *= 2;
// Since C# 14: the type can be left out, the modifier stays
Doubler twice = (ref x) => x *= 2;

int number = 21;
twice(ref number);
Console.WriteLine(number);

delegate void Doubler(ref int value);
