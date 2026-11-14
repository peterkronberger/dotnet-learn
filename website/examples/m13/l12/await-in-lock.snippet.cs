Lock gate = new();

lock (gate)
{
    await Task.Delay(10);
}
