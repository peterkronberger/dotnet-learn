Queue<string> queue = new Queue<string>();
queue.Enqueue("first");
queue.Enqueue("second");
queue.Enqueue("third");

Console.WriteLine(queue.Dequeue());
Console.WriteLine(queue.Peek());
Console.WriteLine(queue.Count);
