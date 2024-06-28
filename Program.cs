using System;
using System.Threading;

class Program
{
    static void Main(string[] args)
    {
        Buffer buffer = new Buffer(10, 4);

        Producer producer1 = new Producer(buffer, 1);
        Producer producer2 = new Producer(buffer, 2);
        Producer producer3 = new Producer(buffer, 3);

        Consumer consumer1 = new Consumer(buffer, 1, new int[] { 1, 2, 3 });
        Consumer consumer2 = new Consumer(buffer, 2, new int[] { 2, 3 });
        Consumer consumer3 = new Consumer(buffer, 3, new int[] { 1, 2 });

        using (producer1)
        using (producer2)
        using (producer3)
        using (consumer1)
        using (consumer2)
        using (consumer3)
        {
            producer1.Start();
            producer2.Start();
            producer3.Start();

            consumer1.Start();
            consumer2.Start();
            consumer3.Start();

            producer1.Dispose();
            producer2.Dispose();
            producer3.Dispose();

            consumer1.Dispose();
            consumer2.Dispose();
            consumer3.Dispose();
        }
        Console.ReadKey();
    }
}
