int attempts = 1;
string word = attempts == 1 ? "attempt" : "attempts";
Console.WriteLine($"{attempts} {word}");
attempts = 5;
word = attempts == 1 ? "attempt" : "attempts";
Console.WriteLine($"{attempts} {word}");
