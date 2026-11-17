[___]
[InlineData("1234567", false)]
[InlineData("12345678", true)]
public void IsValid_ByLength_ReturnsExpected(
    string password, bool expected)
{
    var rule = new PasswordRule(8);

    bool result = rule.IsValid(password);

    Assert.Equal(expected, ___);
}
