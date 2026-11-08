Rule rule = new FailedLoginRule();
Console.WriteLine(rule.Describe());

class Rule
{
    public string Describe() => "base description";
}

class FailedLoginRule : Rule
{
    public new string Describe() => "failed login description";
}
