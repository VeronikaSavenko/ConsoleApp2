using System;
using System.Threading;

class Consumer : IDisposable
{
    private readonly Buffer buffer;
    private readonly int consumerId;
    private readonly int[] acceptedMessages;
    private readonly Thread thread;
    public Consumer(Buffer buffer, int consumerId, int[] acceptedMessages)
    {
        this.buffer = buffer;
        this.consumerId = consumerId;
        this.acceptedMessages = acceptedMessages;
        this.thread = new Thread(Run);
    }
    public void Start()
    {
        thread.Start();
    }
    private void Run()
    {
        try
        {
            while (true)
            {
                int message = buffer.Take();
                if (message == -1)
                {
                    break; // No more items to process
                }
                foreach (int acceptedMessage in acceptedMessages)
                {
                    if (message == acceptedMessage)
                    {
                        Console.WriteLine("Consumer " + consumerId + " consumed: " + message);
                    }
                }
                Thread.Sleep(150); // Simulate time taken to consume
            }
        }
        catch (ThreadInterruptedException)
        {
            Thread.CurrentThread.Interrupt();
        }
    }
    public void Dispose()
    {
        thread.Interrupt();
        thread.Join();
    }
}
