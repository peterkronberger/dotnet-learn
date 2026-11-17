// Before: the result goes to the console
void PrintLevel(int failures)
{
    Console.WriteLine(failures >= 5 ? "high" : "low");
}

// After: the method returns the result
___ LevelOf(___ failures)
{
    return failures >= 5 ? "high" : "low";
}
