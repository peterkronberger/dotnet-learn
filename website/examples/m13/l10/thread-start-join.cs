// A thread created by hand: create it, start it, wait for it.
string message = "";
Thread worker = new(() => message = "done");
worker.Start();
worker.Join();
Console.WriteLine(message);
