using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace os_lab1
{
    public class Thread
    {
        public int ThreadId { get; private set; }

        public int ProcessId { get; private set; }

        public int ThreadExecutionTime { get; private set; } // Оставшееся время выполнения процесса

        public int ThreadOneIterationTime { get; private set; }

        public FormMain Form { get; private set; }

        public Thread(int tId, int pId, FormMain form)
        {
            ThreadId = tId;
            ProcessId = pId;
            Form = form;

            Random rand = new Random();
            ThreadExecutionTime = (rand.Next() % 20) + 10;
            ThreadOneIterationTime = (rand.Next() % 5) + 5;
        }

        public void ExecutionTimeSubtract()
        {
            ThreadExecutionTime -= ThreadOneIterationTime;
            //Form.DrawThread(Form.gr);
        }

        public void Start()
        {
            Form.Threads.Add((ProcessId, ThreadId, ThreadOneIterationTime));
            Form.DrawThread(Form.gr);
        }
    }
}
