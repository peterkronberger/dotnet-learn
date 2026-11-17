[Fact]
public void Require_Empty_ThrowsArgumentException()
{
    var check = new UserCheck();

    var exception = Assert.___<ArgumentException>(
        () => check.Require(""));

    Assert.Equal("name", exception.___);
}
