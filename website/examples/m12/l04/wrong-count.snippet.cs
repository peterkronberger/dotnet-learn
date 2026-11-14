Visit visit = new("bob", "203.0.113.7");
bool found = visit is ("bob", "203.0.113.7", _);

record Visit(string User, string Ip);
