[___]
public void Problems_LongPassword_IsEmpty()
{
    // Arrange
    var rule = new PasswordRule(8);

    // Act
    List<string> problems = rule.Problems("a-long-password");

    // Assert
    Assert.___(problems);
}
