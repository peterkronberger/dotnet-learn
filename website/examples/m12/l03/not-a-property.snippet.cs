Entry entry = new("login-failed", "bob");
bool found = entry is { Missing: 1 };

record Entry(string EventType, string User);
