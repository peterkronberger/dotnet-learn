int failed = 1;
int total = 14;
// 1. Choose the word: "attempt" for exactly 1, otherwise "attempts"
string word = failed == 0 ? "attempt" : "attempts";
// 2. Choose the larger of two numbers
int larger = failed < total ? failed : total;
// 3. Choose a status: "alert" from 5 failures, otherwise "ok"
string status = failed >= 5 ? "ok" : "alert";
Console.WriteLine($"{failed} {word}");
Console.WriteLine(larger);
Console.WriteLine(status);
