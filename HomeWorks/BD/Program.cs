using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;

namespace ConsoleApp48
{
    

    class Program
    {
        static void Main(string[] args)
        {
            Stopwatch stopwatch1 = new Stopwatch();
            stopwatch1.Start();

            string path = Path.Combine(Directory.GetCurrentDirectory(),"bigdata.txt");

            using (var fs = new FileStream(path, FileMode.Create, FileAccess.Write))
            {
                Random rnd = new Random();

                byte[] buffer = new byte[65536];

                for (int i = 0; i < 2000; i++)
                {
                    for (int j = 0; j < buffer.Length; j += 2)
                    {
                        buffer[j] = (byte)rnd.Next(65, 90);
                        buffer[j + 1] = (byte)rnd.Next(97, 122);
                    }

                    fs.Write(buffer, 0, buffer.Length);
                }
            }

            stopwatch1.Stop();

            Console.WriteLine($"Time for Write: {stopwatch1.Elapsed}");



            Stopwatch stopwatch2 = new Stopwatch();
            stopwatch2.Start();

            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read))
            {
                var buffer = new byte[65536];

                long counter = 0;
                int bytesRead;

                while ((bytesRead = fs.Read(buffer, 0, buffer.Length)) > 0)
                {
                    for (int j = 0; j < bytesRead; j++)
                    {
                        if (buffer[j] == 65)
                        {
                            counter++;
                        }
                    }
                }

                Console.WriteLine($"Найдено символов 'A': {counter}");
            }

            stopwatch2.Stop();

            Console.WriteLine($"Time for Read: {stopwatch2.Elapsed}");
        }
    }
}
