string[] addresses = { "a", "b", "a", "a", "a" };
HashSet<string> seen = new HashSet<string>();
int repeated = 0;

foreach (string address in addresses)
{
    if (seen.Add(address))
    {
        repeated++;
    }
}

Console.WriteLine(repeated);
