int failed = 4;
string level = failed switch { < 5 => "medium", < 3 => "low", _ => "high" };
