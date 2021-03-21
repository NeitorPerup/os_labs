using System;

namespace lab2
{
    class Program
    {
        static void Main(string[] args)
        {
            SystemCore systemCore = new SystemCore(new Random().Next() % 3 + 1);

            int TimeWithoutInterrapting = systemCore.toPlanProcessWithoutInterrupting();
            int TimeWithInterrapting = systemCore.toPlanProcessWithInterrupting();

            Console.WriteLine("\n\nЗатраченное время выполнения планировщика без прерываний: " + TimeWithoutInterrapting + "\n\n");
            Console.WriteLine("\n\nЗатраченное время выполнения планировщика c прерываниями: " + TimeWithInterrapting + "\n\n");
        }
    }
}
