// NameList.Parse throws an ArgumentNullException for null
[Fact]
public void Parse_Null_Throws()
{
    var names = new NameList();

    Assert.Throws<ArgumentException>(() => names.Parse(null));
}
