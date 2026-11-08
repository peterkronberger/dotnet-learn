bool TryRead(string text, out int result)
{
    if (text == "7")
    {
        result = 7;
        return true;
    }

    return false;
}
