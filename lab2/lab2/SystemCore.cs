﻿using System;
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

        private bool ProcessEnd = false;

        public SystemCore(int n)
        {
            Processes = new List<Process>();
            Threads = new Dictionary<int, List<Thread>>();
            Random rand = new Random();
            int timeOfOneIteration = rand.Next(3, 10);
            for (int i = 0; i < n; i++)
            {
                Process process = new Process(i, rand.Next(1, 4), timeOfOneIteration);
                Processes.Add(process);
                Threads.Add(i, process.Threads);
            }
        }
        /// <summary>
        /// Метод планирования процессов
        /// </summary>
        public int toPlanProcessWithoutInterrupting()
        {
            var processes = Processes.Select(x => x).ToList();
            var threads = Threads.ToDictionary(x => x.Key, x => x.Value.Select(x => (Thread)x.Clone()).ToList()); // клонируем процессы и потоки

            Console.WriteLine("\n\n" + "НАЧИНАЕМ ПЛАНИРОВАНИЕ ПРОЦЕССОВ БЕЗ ПРЕРЫВАНИЙ!" + "\n\n");

            int fullExecutionTime = 0;

            while (true)
            {
                for (int i = 0; i < processes.Count; i++)
                {

                    processes[i].start(); //Процесс ... начался

                    int temp = toPlanThreadWithoutInterrupting(processes[i].Pid, threads); //temp - затраченное на процесс время

                    fullExecutionTime += temp;

                    //Если затраченное на процесс вреямя равно
                    if (temp == 0)
                    {
                        Console.WriteLine("Удаляем процесс: " + processes[i].Pid);
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

                    int time = threads[pid][i].runWithoutInterrupting();
                    temp += time;

                    if (threads[pid][i].hasInputOutput && threads[pid][i].IOWatingCount >= 0)
                    {
                        Console.WriteLine($"Время ожидания потока: {time}, Осталось взаимодейсвий с устройством ввода/вывода: {threads[pid][i].IOWatingCount}");
                        Console.WriteLine($"Затраченное на поток время: {time}");
                        Console.WriteLine("Затраченное на процесс время: " + temp + ". (Максимальное время итерации процесса " + maxTimeOfThreads + ")" + '\n');
                        if (temp >= maxTimeOfThreads)
                        {
                            Console.WriteLine("Выхожу из планировщика потоков" + '\n');
                            return temp;
                        }
                        break;
                    }

                    Console.WriteLine($"Затраченное на поток время: {time}");
                    Console.WriteLine("Затраченное на процесс время: " + temp + ". (Максимальное время итерации процесса " + maxTimeOfThreads + ")" + '\n');

                    //Если оставшееся время потока меньше либо равно нулю
                    if (threads[pid][i].threadExecutionTime <= 0)
                    {
                        Console.WriteLine("Удаляем поток: " + threads[pid][i].tid);
                        threads[pid].RemoveAt(i);//Удаляем поток
                        i--;
                    }
                    

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
            var threads = Threads.ToDictionary(x => x.Key, x => x.Value.Select(x => (Thread)x.Clone()).ToList()); // клонируем процессы и потоки

            Console.WriteLine("\n\n" + "НАЧИНАЕМ ПЛАНИРОВАНИЕ ПРОЦЕССОВ С ПРЕРЫВАНИЯМИ!" + "\n\n");

            int fullExecutionTime = 0;

            while (true)
            {
                for (int i = 0; i < processes.Count; i++)
                {
                    processes[i].start();//Процесс ... начался

                    int temp = toPlanThreadWithInterrupting(processes[i].Pid, threads);//temp - затраченное на процесс время

                    fullExecutionTime += temp;

                    //Если затраченное на процесс вреямя равно 0
                    if (ProcessEnd)
                    {
                        Console.WriteLine("Удаляем процесс: " + processes[i].Pid);
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
            ProcessEnd = false;
            int temp = 0;//temp - затраченное на процессы время
            while (true)
            {
                for (int i = 0; i < threads[pid].Count; i++)
                {

                    threads[pid][i].start(); //Запускаем поток

                    int resultOfRunning = threads[pid][i].runWithInterrupting(); //Записываем результат запуска потока

                    //Если результат вернул -1 значит поток так и не дождался ввода-вывода
                    if (resultOfRunning == -1)
                    {
                        Console.WriteLine($"Осталось взаимодейсвий с устройством ввода/вывода: {threads[pid][i].IOWatingCount}");
                        Console.WriteLine("Затраченное на процесс время: " + temp + ". (Максимальное время итерации процесса " + maxTimeOfThreads + ")" + '\n');
                        return temp;
                    }
                    else if (resultOfRunning >= 0)
                    {
                        Console.WriteLine($"Затраченное на поток время: {resultOfRunning}");
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

                //Если потоки у данного процесса закончились 
                if (threads[pid].Count == 0)
                {
                    ProcessEnd = true;
                    return temp;
                }
            }
        }
    }
}
