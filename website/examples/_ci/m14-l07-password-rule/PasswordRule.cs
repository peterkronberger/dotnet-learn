namespace PasswordCheck;

public class PasswordRule
{
    public int MinLength { get; }

    public PasswordRule(int minLength)
    {
        MinLength = minLength;
    }

    // Deliberate bug: a password of exactly MinLength characters is rejected
    public bool IsValid(string? password)
    {
        if (password is null)
        {
            return false;
        }
        return password.Length > MinLength;
    }

    public List<string> Problems(string? password)
    {
        var problems = new List<string>();
        if (password is null || password.Length < MinLength)
        {
            problems.Add("too short");
        }
        return problems;
    }

    public string? FirstProblem(string? password)
    {
        List<string> problems = Problems(password);
        return problems.Count == 0 ? null : problems[0];
    }
}
