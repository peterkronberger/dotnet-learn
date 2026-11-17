// UserCheck.Require throws an ArgumentNullException for null
[Fact]
public void Require_Null_Throws()
{
    var check = new UserCheck();

    Assert.Throws<ArgumentException>(() => check.Require(null));
}
