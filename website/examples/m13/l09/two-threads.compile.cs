// Two threads print two lines each. The order of the four lines is not fixed.
Thread threadA = new(() =>
{
    Console.WriteLine("A1");
    Console.WriteLine("A2");
});
Thread threadB = new(() =>
{
    Console.WriteLine("B1");
    Console.WriteLine("B2");
});

threadA.Start();
threadB.Start();

// Join waits until a thread has finished.
threadA.Join();
threadB.Join();
