var line = Console.ReadLine();
int count = 0;
while (line == "ok")
{
    count++;
    line = Console.ReadLine();
}
Console.WriteLine($"{count} ok lines");
