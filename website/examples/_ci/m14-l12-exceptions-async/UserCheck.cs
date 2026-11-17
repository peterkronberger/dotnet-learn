namespace Checks;

public class UserCheck
{
    public string Require(string? name)
    {
        if (name is null)
        {
            throw new ArgumentNullException(nameof(name));
        }
        if (name.Length == 0)
        {
            throw new ArgumentException("The name is empty.", nameof(name));
        }
        return name;
    }
}
