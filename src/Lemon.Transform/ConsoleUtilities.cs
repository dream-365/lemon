using System;

namespace Lemon.Transform
{
    public class ConsoleUtilities
    {
        public static void PrintErrorMessage(string msg)
        {
            var bak = Console.ForegroundColor;

            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine();

            Console.WriteLine(msg);

            Console.ForegroundColor = bak;

            Console.WriteLine();
        }
    }
}
