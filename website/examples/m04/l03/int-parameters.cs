int number = 1;

AddOne(number);
Console.WriteLine($"after value parameter: {number}");
AddOneRef(ref number);
Console.WriteLine($"after ref parameter: {number}");

void AddOne(int value)
{
    value = value + 1;
}

void AddOneRef(ref int value)
{
    value = value + 1;
}
