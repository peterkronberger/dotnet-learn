using System.Text;

string typed = new StringBuilder("Alice").ToString().ToLower();
string stored = "alice";
bool sameText = object.ReferenceEquals(typed, stored);
Console.WriteLine(sameText);
