// 1. Read the user and the number of failed logins
var user = Console.ReadLine();
var text = Console.ReadLine();
int.TryParse(text, out int failed);
// 2. Combine the conditions
bool isWatched = false;
bool isHeavy = false;
bool needsReview = false;
// 3. Print the results
Console.WriteLine($"Watched user: {isWatched}");
Console.WriteLine($"Heavy: {isHeavy}");
Console.WriteLine($"Needs review: {needsReview}");
