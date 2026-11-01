int failed = 7;
// Warning: the switch expression does not handle all values
string severity = failed switch
{
    0 => "none",
    < 3 => "low",
    < 5 => "medium"
};
// At run time, 7 matches no arm and the program stops with an exception
Console.WriteLine(severity);
