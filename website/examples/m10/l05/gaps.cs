// Task 1: change the lambda so that it adds the variable bonus
// Task 2: change bonus to 5 after the lambda is created, then print again
int bonus = 1;
Func<int, int> addBonus = number => number;
Console.WriteLine(addBonus(10));
