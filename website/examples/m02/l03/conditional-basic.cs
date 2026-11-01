int failed = 5;
string level = failed >= 5 ? "high" : "low";
Console.WriteLine(level);
int capped = failed < 3 ? failed : 3;
Console.WriteLine(capped);
