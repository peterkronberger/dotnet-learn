// Task 1: store a lambda in a var; it returns the prefix and the text
//         (parameter text: string, optional parameter prefix: "-")
// Task 2: print the result for "one" and for "two" with the prefix "*"
Console.WriteLine(Combine("-", "start"));

static string Combine(string prefix, string text)
{
    return prefix + " " + text;
}
