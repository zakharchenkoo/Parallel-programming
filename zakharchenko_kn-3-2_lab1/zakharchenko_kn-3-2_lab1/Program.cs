using System;
using System.Threading;
class Program {
    public static long sum1 = 0;
    public static long sum2 = 0;
    public static long sum = 0;
    static void Main() 
    { Console.WriteLine("Захарченко Альона Анатоліївна кн-3-2");
        Thread t1 = new Thread(Actions1);
        Thread t2 = new Thread(Actions2);
        Thread t3 = new Thread(Actions3);
        Thread t4 = new Thread(Actions4);
        Thread t5 = new Thread(Actions5);
        t1.Start();
        t2.Start();
        t3.Start();
        t4.Start();
        t5.Start();
        t1.Join();
        t4.Join();
        sum=sum1 + sum2;
        Console.WriteLine("Загальна сума = " + sum.ToString());
        long chyselnyk = 0;
        chyselnyk = 2 * 1 + (400000 - 1) * 1;
        long perevirka = 0;
        perevirka = (chyselnyk * 400000)/2;
        Console.WriteLine("Перевірка = " + perevirka.ToString());
        if(t5.IsAlive)
        {
            Console.WriteLine("t5 is alive");
        }
        else
        {
            Console.WriteLine("t5 is not alive");
        }
        t5.Join();
        if (t5.IsAlive)
        {
            Console.WriteLine("t5 is alive");
        }
        else
        {
            Console.WriteLine("t5 is not alive");
        }
        Console.WriteLine("Кінець головного потоку");
        Console.Read();
    }
    static void Actions1() 
    {
        Console.WriteLine("t1 starts");
        long a=1;
        for (int i=1; i<=200000; i++)
        {
            sum1=sum1+a;
            a=a+2;
        }
        Console.WriteLine("сума1 = " + sum1.ToString());
        Console.WriteLine("t1 ends\n");
    }
    static void Actions2() 
    {
        Console.WriteLine("t2 starts"); 
        Console.WriteLine("(Actions inside t2)"); 
        Console.WriteLine("t2 ends\n");
    }
    static void Actions3()
    {
        Console.WriteLine("t3 starts"); 
        Console.WriteLine("(Actions inside t3)"); 
        Console.WriteLine("t3 ends\n");
    }
    static void Actions4()
    {
        Console.WriteLine("t4 starts");
        long b=2;
        for (int i = 1; i <= 200000; i++)
        {
            sum2=sum2+b;
            b=b+2;
        }
        Console.WriteLine("сума2 = " + sum2.ToString());
        Console.WriteLine("t4 ends\n");
    }
    static void Actions5() 
    {
        Console.WriteLine("t5 starts");
        Thread.Sleep(16000);
        Console.WriteLine("t5 ends\n");
    }
    
}