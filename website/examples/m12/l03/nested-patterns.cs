// Nested property patterns and the extended form with a dot
Account[] accounts =
[
    new("bob", new Profile(7, "bob@example.com")),
    new("dave", new Profile(2, null)),
    new("erin", null),
];
foreach (Account account in accounts)
{
    bool nested = account is { Profile: { Failures: >= 5 } };
    bool extended = account is { Profile.Failures: >= 5 };
    bool hasMail = account is { Profile.Email: not null };
    Console.WriteLine($"{account.Name}: {nested} {extended} {hasMail}");
}

record Account(string Name, Profile? Profile);
record Profile(int Failures, string? Email);
