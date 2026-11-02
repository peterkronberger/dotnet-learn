int failures = 3;
AddOne(failures);
Console.WriteLine($"After the call: {failures}");
failures = AddedOne(failures);
Console.WriteLine($"After the assignment: {failures}");

void AddOne(int number)
{
    number = number + 1;
    Console.WriteLine($"Inside AddOne: {number}");
}

int AddedOne(int number)
{
    return number + 1;
}
