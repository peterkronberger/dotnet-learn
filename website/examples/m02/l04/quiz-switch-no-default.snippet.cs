int n = 7;
string label = n switch
{
    0 => "zero",
    < 3 => "low",
    < 5 => "medium"
};
