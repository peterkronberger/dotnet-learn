// 1. Set the inputs
int totalMinutes = 32;
int sessions = 5;

// Wrong: both operands are int, so the division drops the decimals
int wrongAverage = totalMinutes / sessions;

// 2. Convert to the fitting type, 3. calculate
double average = (double)totalMinutes / sessions;

// 4. Print the result
Console.WriteLine($"int division: {wrongAverage}");
Console.WriteLine($"double division: {average}");
