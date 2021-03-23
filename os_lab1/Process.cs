using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace os_lab1
{
    public class Process
    {
        public int ProcessId { get; private set; }

        public int ProcessExecutionTime { get; private set; }

        public int ProcessOneItertionTime { get; private set; }

        public FormMain Form { get; private set; }

        public List<Thread> Threads { get; private set; }

        public Process(int id, int ThreadCount, FormMain form)
        {
            ProcessId = id;
            Form = form;
            Threads = new List<Thread>();
            ProcessExecutionTime = 250;
            List<int> ThreadExecutionTimes = RandomThreadExecutionTime(ThreadCount);

            for (int i = 0; i < ThreadCount; ++i)
            {
                Threads.Add(new Thread(i, ProcessId, ThreadExecutionTimes[i], Form)) ;
            }
        }

        public void ProcessExecutionSubtract()
        {
            ProcessExecutionTime -= ProcessOneItertionTime;
        }

        public void toPlan()
        {
            int temp = 0;
            while (true)
            {
                int i = 0;
                for (; i < Threads.Count(); i++)
                {
                    Threads[i].Start();
                    temp += Threads[i].ThreadOneIterationTime;
                    Threads[i].ExecutionTimeSubtract();
                    if (Threads[i].ThreadExecutionTime < 0)
                    {
                        Threads.Remove(Threads[i]);
                        i--;
                    }
                    //Максимальное время для потока
                    int maximumTimeForThreads = 30;
                    if (temp > maximumTimeForThreads)
                    {
                        ProcessOneItertionTime = temp;
                        return;
                    }
                }
                if (Threads.Count() == 0)
                {
                    ProcessOneItertionTime = temp;
                    return;
                }
            }
        }

        private List<int> RandomThreadExecutionTime(int n)
        {
            Random rand = new Random(); // Генератор случайных чисел
            List<int> res = new List<int>(); // возвращаемый список
            int tmp = 0; // записываем сколько времени уже зарандомили
            int contrastTime = ProcessExecutionTime / 15; // минимальное время для потока
            int arrangeTime = ProcessExecutionTime / n;

            for (int i = 0; i < n - 1; ++i)
            {
                int time = rand.Next(arrangeTime - contrastTime, arrangeTime + contrastTime);
                res.Add(time);
                tmp += time;
            }
            res.Add(ProcessExecutionTime - tmp);

            return res;
        }
    }
}
