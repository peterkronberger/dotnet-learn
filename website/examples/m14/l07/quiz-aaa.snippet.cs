[Fact]
public void Describe_ContainsThreshold()
{
    var rule = new FailedLoginRule(5);
    string text = rule.Describe();
    Assert.Equal("Rule: failed logins (threshold 5)", text);
}
