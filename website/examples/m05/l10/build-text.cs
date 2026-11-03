using System.Text;

string[] users = ["alice", "bob", "carol"];
StringBuilder report = new StringBuilder();

foreach (string name in users)
{
    report.AppendLine($"user: {name}");
}

Console.Write(report.ToString());
