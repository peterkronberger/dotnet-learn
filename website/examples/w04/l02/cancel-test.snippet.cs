[Fact]
public async Task CountAsync_CancelledToken_ThrowsCancellation()
{
    var counter = new FailureCounter();
    using var source = new CancellationTokenSource();
    source.Cancel();

    await Assert.ThrowsAsync<OperationCanceledException>(
        () => counter.CountAsync(Lines, source.Token));
}
