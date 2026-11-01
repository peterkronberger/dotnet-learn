int failed = 4;
string label = failed switch
{
    4 => "exactly four",
    > 2 => "more than two",
    _ => "other"
};
Console.WriteLine(label);
