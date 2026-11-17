public bool IsValid(string? password)
{
    if (password is null)
    {
        return false;
    }
    return password.Length >= MinLength;
}
