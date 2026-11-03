string[] array = ["bob", "alice"];
List<string> list = ["carol"];

PrintAll(array);
PrintAll(list);

void PrintAll(IReadOnlyList<string> names)
{
    Console.WriteLine(names.Count);
    foreach (string name in names)
    {
        Console.WriteLine(name);
    }
}
