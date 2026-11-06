// Task 1: create a TextCheck from the method group IsShort
// Task 2: invoke it for "log" and for "authentication" and print both results

static bool IsShort(string text)
{
    return text.Length <= 5;
}

delegate bool TextCheck(string text);
