Rule rule = new FailedLoginRule();
Console.WriteLine(rule.Name());
Console.WriteLine(rule.Describe());

FailedLoginRule specific = new FailedLoginRule();
Console.WriteLine(specific.Name());
Console.WriteLine(specific.Describe());

class Rule
{
    public virtual string Name() => "rule";
    public string Describe() => "base description";
}

class FailedLoginRule : Rule
{
    public override string Name() => "failed-login";
    public new string Describe() => "failed login description";
}
