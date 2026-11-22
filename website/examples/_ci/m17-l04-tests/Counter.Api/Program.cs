var builder = WebApplication.CreateBuilder(args);
builder.Services.AddSingleton<IFailureCounter, FixedFailureCounter>();

var app = builder.Build();

app.MapGet("/failures/{user}", (string user, IFailureCounter counter) =>
    new FailureCount(user, counter.Count(user)));

app.Run();

// Makes the Program class visible to the test project
public partial class Program;

public record FailureCount(string User, int Failures);

public interface IFailureCounter
{
    int Count(string user);
}

public class FixedFailureCounter : IFailureCounter
{
    public int Count(string user) => user == "bob" ? 5 : 0;
}
