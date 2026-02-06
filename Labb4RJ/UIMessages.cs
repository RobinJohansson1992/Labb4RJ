using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4RJ
{
    internal class UIMessages
    {
        public static int CheckInput()
        {
            int userInput;
            while (!int.TryParse(Console.ReadLine(), out userInput))
            {
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("Felaktig inmatning.");
                Console.ResetColor();
            }
            return userInput;
        }
        public static void ErrorMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.WriteLine("Du måste välja ett nummer från listan.");
            Console.ResetColor();
        }
        public static void BackMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("\nTryck enter för att gå tillbaka <-");
            Console.ResetColor();
            Console.ReadKey();
        }
        public static void ExitMessage()
        {
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine("\nProgrammet avslutas...");
            Console.ResetColor();
            Console.ReadKey();
        }
    }
}
