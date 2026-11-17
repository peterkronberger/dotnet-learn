using PasswordCheck;
using Xunit;

namespace PasswordCheck.Tests;

public class PasswordRuleTests
{
    [Fact]
    public void MinLength_AfterCreation_IsTheGivenValue()
    {
        var rule = new PasswordRule(8);

        Assert.Equal(8, rule.MinLength);
    }

    [Fact]
    public void IsValid_LongerThanMinimum_ReturnsTrue()
    {
        // Arrange
        var rule = new PasswordRule(8);

        // Act
        bool result = rule.IsValid("a-long-password");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void IsValid_ExactlyMinimumLength_ReturnsTrue()
    {
        var rule = new PasswordRule(8);

        bool result = rule.IsValid("12345678");

        Assert.True(result);
    }

    [Fact]
    public void IsValid_Null_ReturnsFalse()
    {
        var rule = new PasswordRule(8);

        bool result = rule.IsValid(null);

        Assert.False(result);
    }

    [Fact]
    public void FirstProblem_LongPassword_IsNull()
    {
        var rule = new PasswordRule(8);

        string? problem = rule.FirstProblem("a-long-password");

        Assert.Null(problem);
    }

    [Fact]
    public void FirstProblem_ShortPassword_IsNotNull()
    {
        var rule = new PasswordRule(8);

        string? problem = rule.FirstProblem("abc");

        Assert.NotNull(problem);
    }

    [Fact]
    public void Problems_ShortPassword_ContainsTooShort()
    {
        var rule = new PasswordRule(8);

        List<string> problems = rule.Problems("abc");

        Assert.Contains("too short", problems);
    }

    [Fact]
    public void Problems_LongPassword_IsEmpty()
    {
        var rule = new PasswordRule(8);

        List<string> problems = rule.Problems("a-long-password");

        Assert.Empty(problems);
    }
}
