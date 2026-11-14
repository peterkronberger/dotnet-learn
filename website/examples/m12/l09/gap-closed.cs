// Choose the text by the state of a gate
Gate gate = new Open(40);

string text = gate switch
{
    Shut => "gate is shut",
    Open open => $"gate is {open.Percent} % open",
};
Console.WriteLine(text);

closed record Gate;
sealed record Shut : Gate;
sealed record Open(int Percent) : Gate;
