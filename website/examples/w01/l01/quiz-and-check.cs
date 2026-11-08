int calls = 0;
bool ok = false & Check();
Console.WriteLine(calls);

bool Check()
{
    calls++;
    return true;
}
