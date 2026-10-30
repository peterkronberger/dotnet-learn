double share = 35.714;
Console.WriteLine(share);
// 1. Cut off the decimals with a cast
int cut = 0;
// 2. Round first, then cast
int rounded = 0;
// 3. Add one to the largest int (the value wraps around)
int max = int.MaxValue;
int next = max;
Console.WriteLine(cut);
Console.WriteLine(rounded);
Console.WriteLine(next);
