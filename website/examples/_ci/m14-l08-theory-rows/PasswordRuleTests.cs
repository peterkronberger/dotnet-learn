using PasswordCheck;
using Xunit;

namespace PasswordCheck.Tests;

public class PasswordRuleTests
{
    // One row per case: password length and the expected result
    [Theory]
    [InlineData("1234567", false)]
    [InlineData("12345678", true)]
    [InlineData("123456789", true)]
    [InlineData("", false)]
    public void IsValid_ByLength_ReturnsExpected(string password, bool expected)
    {
        var rule = new PasswordRule(8);

        bool result = rule.IsValid(password);

        Assert.Equal(expected, result);
    }

    // The rows live in a static property
    public static IEnumerable<TheoryDataRow<string?, string?>> FirstProblemRows =>
    [
        new("a-long-password", null),
        new("abc", "too short"),
        new(null, "too short"),
    ];

    [Theory]
    [MemberData(nameof(FirstProblemRows))]
    public void FirstProblem_ByPassword_ReturnsExpected(
        string? password, string? expected)
    {
        var rule = new PasswordRule(8);

        string? problem = rule.FirstProblem(password);

        Assert.Equal(expected, problem);
    }
}
