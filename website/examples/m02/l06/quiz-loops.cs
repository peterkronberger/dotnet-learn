int n = 10;
int runs = 0;
do
{
    runs++;
    n++;
} while (n < 5);
while (n < 5)
{
    runs++;
}
Console.WriteLine($"{n} {runs}");
