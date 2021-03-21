using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    public class Process
    {
        //Идентификатор процесса
        public int pid { get; private set; }

        public Process(int pid, bool displayLabel)
        {
            this.pid = pid;
            if (displayLabel)
            {
                Console.WriteLine("Создаем процесс. PID: " + pid);
            }
        }

        public Thread createThread(int threadsSize)
        {
            return new Thread(threadsSize, pid, new Random().Next() % 2 == 1 ? true : false, true);
        }

        public void start()
        {
            Console.WriteLine("Начинаем процесс. PID: " + pid);
        }
    }
}
