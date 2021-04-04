using System;
using System.Collections.Generic;
using System.Text;

namespace lab2
{
    public class Thread : ICloneable
    {
        //Идентификатор потока
        public int tid { get; private set; }

        //Идентификатор процесса
        public int pid { get; private set; }

        //Оставшееся время выполнения процесса
        public int ThreadExecutionTime;
        public int threadExecutionTime
        {
            get { if (!hasInputOutput) { return ThreadExecutionTime; } else { return IOWaitingTime; } }
            private set { ThreadExecutionTime = value; }
        }

        //Время одной итерации процесса
        public int timeOfOneIteration { get; private set; }

        //Есть ли у этого потока ввод/вывод
        public bool hasInputOutput { get; set; }

        //Время до отклика устройства ввода-вывода
        public int IOWaitingTime { get; private set; }

        public Thread(int tid, int pid, bool hasInputOutput, int OneIteration, int Execution, int IOWating, bool print)
        {
            threadExecutionTime = Execution;
            timeOfOneIteration = OneIteration;
            this.tid = tid;
            this.pid = pid;
            this.hasInputOutput = hasInputOutput;
            if (hasInputOutput)
            {
                IOWaitingTime = IOWating;
            }
            if (print)
            {
                Console.WriteLine("Создаем поток. TID: " + tid + (hasInputOutput ? " Есть ввод/вывод" : " Нет ввода/вывода") + ". Время выполнения "
                + threadExecutionTime + ". Время одной итерации " + timeOfOneIteration);
            }

        }

        //Запуск потока
        public void start()
        {
            Console.WriteLine("Начинаем поток. PID: " + pid + ", TID: " + tid);
            Console.WriteLine("Нужно времени для выполнения: " + (hasInputOutput ? IOWaitingTime : threadExecutionTime));
        }

        //Запуск потока без прерываний
        public int runWithoutInterrupting()
        {
            //Если имеет ввод-вывод
            if (hasInputOutput)
            {
                int spentTime = IOWaitingTime;
                IOWaitingTime = 0;
                return spentTime;
            }
            //Если не имеет ввод вывод
            else
            {
                threadExecutionTime -= timeOfOneIteration;
                return timeOfOneIteration;
            }
        }

        //Запуск потока с прерываниями
        public int runWithInterrupting()
        {
            //Если имеет ввод-вывод
            if (hasInputOutput)
            {

                if (IOWaitingTime > timeOfOneIteration)
                {
                    IOWaitingTime -= timeOfOneIteration;
                    return -1;
                }
                else
                {
                    int spentTime = IOWaitingTime;
                    IOWaitingTime = 0;
                    return spentTime;
                }

            }
            //Если не имеет ввод вывод
            else
            {
                threadExecutionTime -= timeOfOneIteration;
                return timeOfOneIteration;
            }
        }

        public int waitIO(int time)
        {
            return IOWaitingTime -= time;
        }

        public object Clone()
        {
            return new Thread(tid, pid, hasInputOutput, timeOfOneIteration, threadExecutionTime, IOWaitingTime, false);
        }
    }
}
