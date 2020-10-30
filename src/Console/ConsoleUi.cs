using System;

namespace EumelConsole
{
    class ConsoleUi
    {
        public static int PromptInt(string prompt, int min, int max)
        {
            do
            {
                var oldFc = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.Write(prompt);
                Console.ForegroundColor = oldFc;
                var isInteger = int.TryParse(System.Console.ReadLine(), out int enteredInt);

                if (!isInteger || enteredInt < min || enteredInt > max)
                {
                    Console.WriteLine($" -> Enter an integer number between {min} and {max}.");
                    continue;
                }
                return enteredInt;
            } while (true);
        }
    }
}