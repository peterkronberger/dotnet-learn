string[] ips = ["192.0.2.7", "198.51.100.4", "192.0.2.7"];
HashSet<string> seen = [];
int distinct = 0;

foreach (string ip in ips)
{
    if (seen.Add(ip))
    {
        distinct++;
    }
}

Console.WriteLine(distinct);
