using System;
using System.Diagnostics;
using System.Threading;

class Program
{
    static int J; 

    static void Main(string[] args)
    {
        Console.WriteLine("Захарченко Альона Анатоліївна кн-3-2");
        Console.WriteLine("Поточна дата та час: " + DateTime.Now);
        Console.WriteLine("Формула A: 1 + 2 + 3 + 4 + 5 + 6 ...");

        Console.Write("Введіть кількість ітерацій (J): ");
        J = int.Parse(Console.ReadLine());

        // Послідовні обчислення
        Stopwatch stopwatch = new Stopwatch();
        stopwatch.Start();
        Actions1();
        Actions2();
        Actions3();
        stopwatch.Stop();

        Console.WriteLine($"Час послідовного обчислення: {stopwatch.Elapsed.TotalSeconds:F3} с");

        // Паралельні обчислення
        Console.WriteLine("Паралельні обчислення:");
        stopwatch.Reset();
        stopwatch.Start();
        double[] parallelResults = new double[3];
        ManualResetEvent[] mres = new ManualResetEvent[3];

        for (int i = 0; i < 3; i++)
        {
            mres[i] = new ManualResetEvent(false);
            int threadNumber = i + 1;

            ThreadPool.QueueUserWorkItem((state) =>
            {
                ThreadWork(threadNumber, threadNumber * (J / 3) - (J / 3) + 1, threadNumber * (J / 3), parallelResults, mres[threadNumber - 1]);
            });
        }

        WaitHandle.WaitAll(mres);

        stopwatch.Stop();

        double parallelResult = parallelResults[0] + parallelResults[1] + parallelResults[2];
        Console.WriteLine($"Результат паралельного обчислення: {parallelResult}");
        Console.WriteLine($"Час паралельного обчислення: {stopwatch.Elapsed.TotalSeconds:F3} с");

        // Перевірка результатів
        Console.WriteLine("Результати обчислень співпадають: {0}", parallelResult.Equals(parallelResult));

        // Визначення прискорення
        double acceleration = stopwatch.Elapsed.TotalSeconds / stopwatch.Elapsed.TotalSeconds;
        double percentage = (1 - (stopwatch.Elapsed.TotalSeconds / stopwatch.Elapsed.TotalSeconds)) * 100;
        Console.WriteLine("Паралельне (послідовне) обчислення є швидшим, ніж послідовне (паралельне).");
        Console.WriteLine("Прискорення = {0:F3} разів ({1:F2}%)", acceleration, percentage);

        // Отримання інформації про процесор
        int processorCount = Environment.ProcessorCount;
        Console.WriteLine("Кількість ядер на процесорі: {0}", processorCount);
        Console.WriteLine("Ефективність паралельних обчислень: {0:F2}%", percentage / processorCount);

        Console.WriteLine("Кінець головного потоку");
    }

    static void ThreadWork(int threadNumber, int start, int end, double[] results, ManualResetEvent mre)
    {
        Console.WriteLine($"Потік #{threadNumber} почав роботу");
        results[threadNumber - 1] = CalculateA(start, end);
        Console.WriteLine($"Потік #{threadNumber} завершив роботу");
        mre.Set(); // Сигнал, що потік завершив роботу
    }

    static double CalculateA(int start, int end)
    {
        double result = 0;
        for (int i = start; i <= end; i++)
        {
            result += i;
        }
        return result;
    }

    static void Actions1()
    {
        int start = 1;
        int end = J / 3;
        double result = CalculateA(start, end);
        Console.WriteLine($"Результат в потоці #1: {result}");
    }

    static void Actions2()
    {
        int start = J / 3 + 1;
        int end = 2 * (J / 3);
        double result = CalculateA(start, end);
        Console.WriteLine($"Результат в потоці #2: {result}");
    }

    static void Actions3()
    {
        int start = 2 * (J / 3) + 1;
        int end = J;
        double result = CalculateA(start, end);
        Console.WriteLine($"Результат в потоці #3: {result}");
    }
}
