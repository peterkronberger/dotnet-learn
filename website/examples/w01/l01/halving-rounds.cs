int total = 7;
int rounds = 0;

do
{
    total = total / 2;
    rounds++;
    Console.WriteLine($"Round {rounds}: {total}");
}
while (total > 0);

Console.WriteLine($"Rounds: {rounds}");
