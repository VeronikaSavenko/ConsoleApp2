using System;
using System.Collections.Generic;
using System.Threading;

class Buffer
{
    private readonly Queue<int> queue = new Queue<int>();
    private readonly int capacity;
    private readonly object lockObject = new object();
    private readonly ManualResetEventSlim notFull = new ManualResetEventSlim(true);
    private readonly ManualResetEventSlim notEmpty = new ManualResetEventSlim(false);
    private int producersDone = 0;
    private readonly int totalProducers;

    public Buffer(int capacity, int totalProducers)
    {
        this.capacity = capacity;
        this.totalProducers = totalProducers;
    }

    public void Put(int value)
    {
        lock (lockObject)
        {
            while (queue.Count == capacity)
            {
                Console.WriteLine("Buffer is full. Producer is waiting.");
                notFull.Reset();
                Monitor.Wait(lockObject);
            }
            queue.Enqueue(value);
            Console.WriteLine("Produced: " + value + " | Buffer: " + string.Join(", ", queue));
            notEmpty.Set();
            Monitor.PulseAll(lockObject);
        }
    }

    public int Take()
    {
        lock (lockObject)
        {
            while (queue.Count == 0)
            {
                if (producersDone == totalProducers)
                {
                    return -1; // Signal that no more items will be produced
                }
                Console.WriteLine("Buffer is empty. Consumer is waiting.");
                notEmpty.Reset();
                Monitor.Wait(lockObject);
            }
            int value = queue.Dequeue();
            Console.WriteLine("Consumed: " + value + " | Buffer: " + string.Join(", ", queue));
            notFull.Set();
            Monitor.PulseAll(lockObject);
            return value;
        }
    }

    public void ProducerDone()
    {
        lock (lockObject)
        {
            producersDone++;
            Console.WriteLine("Producer finished. Total finished: " + producersDone);
            notEmpty.Set();
            Monitor.PulseAll(lockObject);
        }
    }
}
