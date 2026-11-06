List<int> numbers = [4, 8, 15];
List<string> names = ["ann", "bob"];
Console.WriteLine(LastOf(numbers));
// Task 2: print the last name with LastOf, give the type argument yourself

// Task 1: make LastOf generic, so that it works for List<string> too
static int LastOf(List<int> items)
{
    return items[items.Count - 1];
}
