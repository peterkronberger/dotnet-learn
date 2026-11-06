// Task 1: replace the helper IsShort by a lambda in the call of FindAll
// Task 2: print the result with string.Join(", ", ...)
List<string> words = ["log", "incident", "rule", "ip"];

var shortWords = words.FindAll(IsShort);

static bool IsShort(string word)
{
    return word.Length <= 3;
}
