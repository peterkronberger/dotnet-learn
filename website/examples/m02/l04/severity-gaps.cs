int failed = 4;
// Step 1: add the arms 0 => "none", < 3 => "low" and < 5 => "medium"
string severity = failed switch
{
    _ => "high",
};
Console.WriteLine(severity);
