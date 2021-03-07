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

        public Process(int id, FormMain form)
        {
            ProcessId = id;
            Form = form;
            Threads = new List<Thread>();
            ProcessExecutionTime = 0;

            int n = new Random().Next() % 3 + 1; // количество потоков
            for (int i = 0; i < n; ++i)
            {
                Threads.Add(new Thread(i, ProcessId, Form));
                ProcessExecutionTime += Threads[i].ThreadExecutionTime;
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
                for (int i = 0; i < Threads.Count(); i++)
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
    }
}
