using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    public class Process
    {
        //Идентификатор процесса
        public int Pid { get; private set; }

        public List<Thread> Threads { get; private set; }

        public Process(int pid, int n)
        {
            Pid = pid;
            Threads = new List<Thread>();
            Random rand = new Random();
            Console.WriteLine("Создаем процесс. PID: " + pid + " Количество потоков: " + n);
            for (int i = 0; i < n; i++)
            {
                bool hasIO = rand.Next(0, 2) == 1 ? true : false;
                int timeOfOneIteration = rand.Next(3, 10);
                int threadExecutionTime = rand.Next(10, 30);
                int IOWaitingTime = rand.Next(10, 30);
                Threads.Add(new Thread(i, pid, hasIO, timeOfOneIteration, threadExecutionTime, IOWaitingTime, true));
            }
        }

        public void start()
        {
            Console.WriteLine("Начинаем процесс. PID: " + Pid);
        }
    }
}
