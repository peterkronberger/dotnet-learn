[Fact]
public void IsValid_Null_ReturnsFalse()
{
    var rule = new PasswordRule(8);
    Assert.False(rule.IsValid(null));
}

public void IsValid_Empty_ReturnsFalse()
{
    var rule = new PasswordRule(8);
    Assert.False(rule.IsValid(""));
}
