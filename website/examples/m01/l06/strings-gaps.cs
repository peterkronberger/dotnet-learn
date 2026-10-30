string user = "alice";
int count = 3;
double rate = 42.857;
// 1. Join text and a number with interpolation
Console.WriteLine($"{user} failed {count} times");
// 2. Format the number with one decimal place
Console.WriteLine($"Rate: {rate:F0} %");
// 3. Insert the number of characters of the text
Console.WriteLine($"{user} has {0} letters");
