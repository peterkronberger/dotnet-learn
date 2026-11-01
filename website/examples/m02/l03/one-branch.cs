int checks = 0;
bool fast = true;
// 1. The condition is true: only the first branch is evaluated
int result = fast ? 10 : ++checks;
Console.WriteLine($"result={result}, checks={checks}");
// 2. The condition is false: only the second branch is evaluated
fast = false;
result = fast ? 10 : ++checks;
Console.WriteLine($"result={result}, checks={checks}");
