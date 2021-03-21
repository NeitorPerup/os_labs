using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;

namespace lab2
{
    public class SystemCore
    {
        List<Process> Processes;//Список процессов
        Dictionary<int, List<Thread>> Threads;//Словарь для потоков (ключ PID) значение список потоков

        //максимальное время на потоки (на один процесс)
        private int maxTimeOfThreads = 30;

        List<Thread> blockedThreads = new List<Thread>();//Заблокирлованные потоки

        public SystemCore(int n)
        {
            Processes = new List<Process>();
            Threads = new Dictionary<int, List<Thread>>();
           
            for (int i = 0; i < n; i++)
            {
                Processes.Add(new Process(Processes.Count, true));
                createThreads(Processes.Count - 1);
            }
        }

        /// <summary>
        /// Метод для создания новых потоков
        /// <param name="pid"> идентификатор процесса </param> 
        /// </summary>
        public void createThreads(int pid)
        {
            List<Thread> threads = new List<Thread>();
            for (int i = 0; i < new Random().Next() % 2 + 1; i++)
            {
                threads.Add(Processes[Processes.Count - 1].createThread(threads.Count));
            }
            Threads.Add(pid, threads);
        }

        /// <summary>
        /// Метод планирования процессов
        /// </summary>
        public int toPlanProcessWithoutInterrupting()
        {
            var processes = Processes.Select(x => x).ToList();
            var threads = Threads.ToDictionary(x => x.Key, x => x.Value.Select(x => (Thread)x.Clone()).ToList());

            Console.WriteLine("\n\n" + "НАЧИНАЕМ ПЛАНИРОВАНИЕ ПРОЦЕССОВ БЕЗ ПРЕРЫВАНИЙ!" + "\n\n");

            int fullExecutionTime = 0;

            while (true)
            {
                for (int i = 0; i < processes.Count; i++)
                {

                    processes[i].start(); //Процесс ... начался

                    int temp = toPlanThreadWithoutInterrupting(processes[i].pid, threads); //temp - затраченное на процесс время

                    fullExecutionTime += temp;

                    //Если затраченное на процесс вреямя равно
                    if (temp == 0)
                    {
                        Console.WriteLine("Удаляем процесс: " + processes[i].pid);
                        processes.RemoveAt(i); //Удаляем этот процесс
                        break;
                    }
                }

                //Если размер списка процессов равен нулю
                if (processes.Count == 0)
                {
                    return fullExecutionTime;
                }
            }
        }

        /// <summary>
        /// Метожд планирования потоков
        /// </summary>
        public int toPlanThreadWithoutInterrupting(int pid, Dictionary<int, List<Thread>> threads)
        {

            int temp = 0;//temp - затраченное на процессы время
            while (true)
            {
                for (int i = 0; i < threads[pid].Count; i++)
                {

                    threads[pid][i].start();//Запускаем поток

                    temp += threads[pid][i].runWithoutInterrupting();

                    //Если оставшееся время потока меньше либо равно нулю
                    if (threads[pid][i].threadExecutionTime <= 0)
                    {
                        Console.WriteLine("Удаляем поток: " + threads[pid][i].tid);
                        threads[pid].RemoveAt(i);//Удаляем поток
                        i--;
                    }

                    Console.WriteLine("Затраченное на процесс время: " + temp + ". (Максимальное время итерации процесса " + maxTimeOfThreads + ")" + '\n');

                    //Если затраченное на процессы время превысило максимальное допустимое вресмя на один процесс
                    if (temp >= maxTimeOfThreads)
                    {
                        Console.WriteLine("Выхожу из планировщика потоков" + '\n');
                        return temp;
                    }
                }

                //Если потоки у данного процесса закончились
                if (threads[pid].Count == 0)
                {
                    return temp;
                }
            }
        }

        //Метод планирования процессов
        public int toPlanProcessWithInterrupting()
        {
            var processes = Processes.Select(x => x).ToList();
            var threads = Threads.ToDictionary(x => x.Key, x => x.Value.Select(x => (Thread)x.Clone()).ToList());

            Console.WriteLine("\n\n" + "НАЧИНАЕМ ПЛАНИРОВАНИЕ ПРОЦЕССОВ С ПРЕРЫВАНИЯМИ!" + "\n\n");

            int fullExecutionTime = 0;

            while (true)
            {
                for (int i = 0; i < processes.Count; i++)
                {
                    processes[i].start();//Процесс ... начался

                    int temp = toPlanThreadWithInterrupting(processes[i].pid, threads);//temp - затраченное на процесс время

                    fullExecutionTime += temp;

                    //Если затраченное на процесс вреямя равно 0
                    if (temp == 0)
                    {
                        Console.WriteLine("Удаляем процесс: " + processes[i].pid);
                        processes.RemoveAt(i);//Удаляем этот процесс
                        break;
                    }
                }

                //Если размер списка процессов равен нулю
                if (processes.Count == 0)
                {
                    return fullExecutionTime;
                }
            }
        }

        /// <summary>
        /// Метожд планирования потоков
        /// param pid Идентификатор процесса
        /// return Время затраченное на потоки
        /// </summary>
        public int toPlanThreadWithInterrupting(int pid, Dictionary<int, List<Thread>> threads)
        {

            int temp = 0;//temp - затраченное на процессы время
            while (true)
            {
                for (int i = 0; i < threads[pid].Count; i++)
                {
                    //Если этот процесс находится в заблокированных
                    if (blockedThreads.Contains(threads[pid][i]))
                    {
                        if (threads[pid][i].threadExecutionTime <= 0)
                        {
                            blockedThreads.Remove(threads[pid][i]);
                            Console.WriteLine("Удаляем поток: " + threads[pid][i].tid);
                            threads[pid].RemoveAt(i);
                            i--;
                        }
                        //переходим к планировке следующих процессов
                        continue;
                    }

                    threads[pid][i].start(); //Запускаем поток
                    
                    int resultOfRunning = threads[pid][i].runWithInterrupting(); //Записываем результат запуска потока
                    
                    foreach (Thread blockedThread in blockedThreads) //Тем временем заблокированные потоки ждут ввода-вывода
                    {
                        blockedThread.waitIO(resultOfRunning);
                    }

                    //Если результат вернул -1 значит поток так и не дождался ввода-вывода
                    if (resultOfRunning == -1)
                    {
                        temp += threads[pid][i].timeOfOneIteration;
                        blockedThreads.Add(threads[pid][i]); //Добавляем этот поток в заблокированные
                    }
                    else if (resultOfRunning >= 0)
                    {
                        temp += resultOfRunning;
                    }

                    //Если оставшееся время потока меньше либо равно нулю
                    if (threads[pid][i].threadExecutionTime <= 0)
                    {
                        Console.WriteLine("Удаляем поток: " + threads[pid][i].tid);
                        threads[pid].RemoveAt(i);//Удаляем поток
                        i--;
                    }

                    Console.WriteLine("Затраченное на процесс время: " + temp + ". (Максимальное время итерации процесса " + maxTimeOfThreads + ")" + '\n');

                    //Если затраченное на процессы время превысило максимальное допустимое вресмя на один процесс
                    if (temp >= maxTimeOfThreads)
                    {
                        Console.WriteLine("Выхожу из планировщика потоков" + '\n');
                        return temp;
                    }
                }

                bool existThreadWithoutBlocking = false;

                //Ищем есть ли поток без блокировки
                for (int i = 0; i < threads[pid].Count; i++)
                {
                    if (!blockedThreads.Contains(threads[pid][i]))
                    {
                        existThreadWithoutBlocking = true;
                        break;
                    }
                }

                //Если потоки у данного процесса закончились или нет потоков без блокировки
                if (threads[pid].Count == 0 || !existThreadWithoutBlocking)
                {
                    return temp;
                }
            }
        }
    }
}
