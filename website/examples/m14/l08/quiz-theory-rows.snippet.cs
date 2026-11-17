[Theory]
[InlineData(7, false)]
[InlineData(8, true)]
[InlineData(9, true)]
public void IsLongEnough_ByLength_ReturnsExpected(int length, bool expected)
{
    var rule = new LengthRule(8);

    bool result = rule.IsLongEnough(length);

    Assert.Equal(expected, result);
}
