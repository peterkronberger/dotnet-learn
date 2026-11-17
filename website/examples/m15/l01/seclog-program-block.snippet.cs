// Composition root: the readers and what they need are put together here
IReportWriter reportWriter = new ConsoleReportWriter();
LogSourceReader webReader = new("web", logSources["web"], reportWriter);
LogSourceReader vpnReader = new("vpn", logSources["vpn"], reportWriter);
// Release a log source reader asynchronously
await using (LogSourceReader reader = webReader)
{
    Console.WriteLine($"Failures in {reader.Name}: {reader.CountFailures()}");
}
{
    await using LogSourceReader reader = vpnReader;
    Console.WriteLine($"Failures in {reader.Name}: {reader.CountFailures()}");
    Console.WriteLine("End of the block");
}
