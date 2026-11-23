// SourceReader.ReadAsync throws an InvalidOperationException for ""
[Fact]
public async Task ReadAsync_EmptySource_Throws()
{
    var reader = new SourceReader();

    Assert.ThrowsAsync<InvalidOperationException>(() => reader.ReadAsync(""));
}
