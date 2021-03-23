using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace os_lab1
{
    public class SystemCore
    {
        public List<Process> Processes { get; private set; }

        public FormMain Form { get; private set; }

        public SystemCore(FormMain form)
        {
            Form = form;
            Processes = new List<Process>();
        }

        public void toPlan()
        {
            int temp = 0;
            while (true)
            {
                for (int i = 0; i < Processes.Count(); i++)
                {
                    Processes[i].toPlan();
                    temp += Processes[i].ProcessOneItertionTime;
                    Processes[i].ProcessExecutionSubtract();
                    if (Processes[i].ProcessExecutionTime < 0)
                    {
                        Processes.Remove(Processes[i]);
                        i--;
                    }
                    //Максимальное время для процесса
                    int maximumTimeForProcesses = 350;
                    if (temp > maximumTimeForProcesses)
                    {
                        return;
                    }
                }
                if (Processes.Count() == 0)
                {
                    return;
                }
            }
        }

        public void Start()
        {
            Random rand = new Random();
            int n = rand.Next(6, 11);
            for (int i = 0; i < n; ++i)
            {
                Processes.Add(new Process(i, rand.Next(1, 5), Form));
            }
            toPlan();
        }
    }
}
