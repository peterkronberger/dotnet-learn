var text = Console.ReadLine();
int.TryParse(text, out int failed);
string severity = failed switch
{
    0 => "none",
    < 3 => "low",
    < 5 => "medium",
    _ => "high"
};
Console.WriteLine($"{failed} failures: {severity}");
