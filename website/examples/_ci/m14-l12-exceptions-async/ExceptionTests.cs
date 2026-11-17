using Xunit;

namespace Checks.Tests;

public class ExceptionTests
{
    [Fact]
    public void Require_Null_ThrowsArgumentNullException()
    {
        var check = new UserCheck();

        var exception = Assert.Throws<ArgumentNullException>(
            () => check.Require(null));

        Assert.Equal("name", exception.ParamName);
    }

    [Fact]
    public void Require_Empty_ThrowsArgumentException()
    {
        var check = new UserCheck();

        var exception = Assert.Throws<ArgumentException>(
            () => check.Require(""));

        Assert.Equal("name", exception.ParamName);
    }

    // Fails on purpose: Throws needs the exact type, not a derived one
    [Fact]
    public void Require_Null_ThrowsExactArgumentException()
    {
        var check = new UserCheck();

        Assert.Throws<ArgumentException>(() => check.Require(null));
    }

    [Fact]
    public void Require_Null_ThrowsAnyArgumentException()
    {
        var check = new UserCheck();

        Assert.ThrowsAny<ArgumentException>(() => check.Require(null));
    }
}
