Severity level = Severity.Medium;
Console.WriteLine(level);
Console.WriteLine((int)level);
Console.WriteLine(level > Severity.Low);
Severity fromNumber = (Severity)3;
Console.WriteLine(fromNumber);

enum Severity { None, Low, Medium, High }
