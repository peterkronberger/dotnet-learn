var log = (string text, string level = "INFO") => $"[{level}] {text}";
Func<string, string, string> same = log;
