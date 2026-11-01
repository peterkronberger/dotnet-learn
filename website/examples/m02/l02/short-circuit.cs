int calls = 0;
int failed = 2;
bool shortCircuit = failed > 5 && ++calls > 0;
Console.WriteLine($"&&: {shortCircuit}, calls: {calls}");
bool both = failed > 5 & ++calls > 0;
Console.WriteLine($"&: {both}, calls: {calls}");
