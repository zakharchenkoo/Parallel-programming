using System;
using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Ласкаво прошу в програму для обробки текстових і числових даних!");
        Console.WriteLine("Автор: Захарченко Альона кн-3-2");
        Console.WriteLine();

        bool continueProcessing = true;

        while (continueProcessing)
        {
            Console.WriteLine("Введіть текст або число (не більше 50 символів):");
            string input = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(input) && IsValidInput(input))
            {
                Task.Run(() => ProcessInput(input));
            }
            else if (string.IsNullOrWhiteSpace(input))
            {
                Console.WriteLine("Ви ввели пустий рядок. Будь ласка, спробуйте знову.");
            }
            else
            {
                Console.WriteLine("Некоректне введення або перевищено ліміт символів (50). Будь ласка, спробуйте ще раз.");
            }

            Console.WriteLine("Бажаєте ввести інші дані? (Yes/No)");
            string response = Console.ReadLine();

            if (response.Equals("No", StringComparison.OrdinalIgnoreCase))
            {
                continueProcessing = false;
            }
        }

        Console.WriteLine("Дякую за використання програми. Натисніть будь-яку клавішу для завершення");
        Console.ReadKey();
    }

    static void ProcessInput(string input)
    {
        if (IsNumeric(input))
        {
            double number = double.Parse(input);
            double square = number * number;
            Console.WriteLine($"Квадрат числа {number} дорівнює {square}");
        }
        else
        {
            Console.WriteLine("Введений текст має довжину " + input.Length + " символів.");
        }
    }

    static bool IsNumeric(string input)
    {
        return double.TryParse(input, out _);
    }

    static bool IsValidInput(string input)
    {
        if (input.Length > 50)
            return false;

        return true;
    }
}
