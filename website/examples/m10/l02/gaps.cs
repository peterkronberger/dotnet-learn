string[] words = ["log", "incident", "rule"];

// Task 1: write the method CountWhere(string[] values, Func<string, bool> test)
// that returns how many values pass the test
// Task 2: print CountWhere(words, IsLong)

static bool IsLong(string word)
{
    return word.Length > 3;
}
