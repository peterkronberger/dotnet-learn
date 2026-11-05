// Guard clauses with throw helpers: check the arguments first
Report(null, 3);
Report("bob", -1);
Report("", 2);
Report("bob", 5);

void Report(string? user, int failures)
{
    try
    {
        Check(user, failures);
        Console.WriteLine($"{user}: {failures} failures");
    }
    catch (ArgumentNullException ex)
    {
        Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.WriteLine($"{ex.GetType().Name}: negative count");
    }
    catch (ArgumentException ex)
    {
        Console.WriteLine($"{ex.GetType().Name}: {ex.Message}");
    }
}

void Check(string? user, int failures)
{
    ArgumentNullException.ThrowIfNull(user);
    ArgumentException.ThrowIfNullOrEmpty(user);
    ArgumentOutOfRangeException.ThrowIfNegative(failures);
}
