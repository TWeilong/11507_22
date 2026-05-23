using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    public class QueueWorker
    {
        private Queue<Func<int>> queue =new Queue<Func<int>>();
        private int total = 0;
        private object queueLock = new object(); private object sumLock =new object();
        public void Start()
        {
            for (int i = 0; i < 100; i++)
            {
                queue.Enqueue(() =>
                {
                    return 1;
                });
            }
            List<Task> tasks =new List<Task>();

            for (int i = 0; i < 3; i++)
            {
                tasks.Add(Task.Run(() =>
                {
                    while (true)
                    {
                        Func<int> job = null;
                        lock (queueLock)
                        {
                            if (queue.Count > 0)
                            {
                                job = queue.Dequeue();
                            }
                            else
                            {
                                break;
                            }
                        }
                        int result = job();
                        lock (sumLock)
                        {total += result;} }}));
            }
            Task.WaitAll(tasks.ToArray());
            Console.WriteLine(total);
        }
    }


}
