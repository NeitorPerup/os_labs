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

        public int ThreadExecutionTime { get; set; } // Оставшееся время выполнения процесса

        public int ThreadOneIterationTime { get; private set; }

        public FormMain Form { get; private set; }

        public ThreadStatusEnum? Status { get; set; } // null - не последний, false - последний(процесс будет продолжен) ,true - последний(процесс завершается) 

        public Thread(int tId, int pId, int time, FormMain form)
        {
            ThreadId = tId;
            ProcessId = pId;
            Form = form;
            ThreadExecutionTime = time;

            Random rand = new Random();
            ThreadOneIterationTime = ThreadExecutionTime / rand.Next(10, 18);
        }

        public void ExecutionTimeSubtract()
        {
            ThreadExecutionTime -= ThreadOneIterationTime;
        }

        public void Start()
        {
            Form.Threads.Add(new Thread(ThreadId, ProcessId, ThreadExecutionTime, Form)
            {
                ThreadOneIterationTime = this.ThreadOneIterationTime,
                Status = this.Status
            });
            Form.AddMark();
        }
    }
}
