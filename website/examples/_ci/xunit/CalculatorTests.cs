using Xunit;

// Demo for spike S2: test runner output as a snapshot.
public class CalculatorTests
{
    [Fact]
    public void Add_TwoNumbers_ReturnsSum()
    {
        Assert.Equal(5, 2 + 3);
    }

    [Theory]
    [InlineData(7, 2, 3)]
    [InlineData(-7, 2, -3)]
    public void Divide_Integers_TruncatesTowardZero(int a, int b, int expected)
    {
        Assert.Equal(expected, a / b);
    }
}
