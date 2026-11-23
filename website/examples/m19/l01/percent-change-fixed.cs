int minutesWithoutAi = 40;
int minutesWithAi = 50;

double change = (minutesWithAi - minutesWithoutAi) * 100.0 / minutesWithoutAi;

Console.WriteLine($"Time change: {change} %");
