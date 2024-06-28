using System;
using System.Threading;

class Producer : IDisposable
{
    private readonly Buffer buffer;
    private readonly int producerId;
    private readonly Thread thread;

    public Producer(Buffer buffer, int producerId)
    {
        this.buffer = buffer;
        this.producerId = producerId;
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
            for (int i = 1; i <= 4; i++)
            {
                Console.WriteLine("Producer " + producerId + " produced: " + i);
                buffer.Put(i);
                Thread.Sleep(100); // Simulate time taken to produce
            }
        }
        catch (ThreadInterruptedException)
        {
            Thread.CurrentThread.Interrupt();
        }
        finally
        {
            buffer.ProducerDone();
        }
    }

    public void Dispose()
    {
        thread.Interrupt();
        thread.Join();
    }
}
