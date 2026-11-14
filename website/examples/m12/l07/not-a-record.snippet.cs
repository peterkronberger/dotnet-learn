Account account = new() { Owner = "bob" };
Account other = account with { Owner = "dave" };

class Account
{
    public string Owner { get; set; } = "";
}
