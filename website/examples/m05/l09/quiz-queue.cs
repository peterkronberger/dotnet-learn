Queue<int> queue = new Queue<int>();
Stack<int> stack = new Stack<int>();

for (int i = 1; i <= 3; i++)
{
    queue.Enqueue(i);
    stack.Push(i);
}

queue.Dequeue();
stack.Pop();
Console.WriteLine(queue.Peek());
Console.WriteLine(stack.Peek());
Console.WriteLine(queue.Count + stack.Count);
