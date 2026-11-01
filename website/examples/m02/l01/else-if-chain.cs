// 1. Read the number of failed logins
var text = Console.ReadLine();
int.TryParse(text, out int failed);
// 2. Choose the level: the first true condition wins
string level;
if (failed >= 5)
{
    level = "high";
}
else if (failed >= 3)
{
    level = "medium";
}
else
{
    level = "low";
}
// 3. Print the result
Console.WriteLine($"Level: {level}");
