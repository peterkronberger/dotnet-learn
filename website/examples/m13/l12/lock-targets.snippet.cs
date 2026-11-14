// Good: a private object that only this class knows
private readonly Lock _gate = new();

// Bad: reachable from outside, other code may lock on it too
lock (this) { }
lock (typeof(Counter)) { }
lock ("counter") { }
