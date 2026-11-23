[Fact]
public async void CountAsync_ThreeLines_ReturnsTwo()
{
    var counter = new FailureCounter();

    int result = await counter.CountAsync(
        Lines, TestContext.Current.CancellationToken);

    Assert.Equal(2, result);
}
