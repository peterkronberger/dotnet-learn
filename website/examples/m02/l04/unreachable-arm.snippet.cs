int failed = 7;
string severity = failed switch
{
    >= 3 => "medium",
    >= 5 => "high",
    _ => "low",
};
