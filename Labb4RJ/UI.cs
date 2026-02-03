using Labb4RJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4RJ
{
    internal class UI
    {
        public static void PrintClassesUI()
        {
            Console.WriteLine("Välj klass från listan:\n");

        }
        public static void PrintStudentsUI()
        {
            Console.WriteLine("Visa elever:\n");
            Console.WriteLine("" +
                "1. Förnamn stigande \n" +
                "2. Förnamn fallande \n" +
                "3. Efternamn stigande \n" +
                "4. Efternamn fallande \n" +
                "\n0. Tillbaka <-");
        }
        public static void ErrorMessage()
        {
            Console.WriteLine("Du måste välja ett nummer från listan.");
        }
        public static void BackToMainMessage()
        {
            Console.WriteLine("\nTryck enter för att återgå till huvudmenyn.");
        }
        public static void ExitMessage()
        {
            Console.WriteLine("\nProgrammet avslutas...");
            Console.ReadKey();
        }
        public static void StaffUI(Labb4Context context)
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.WriteLine("Personal\n");
                Console.WriteLine("" +
                    "1. Visa personal \n" +
                    "2. Visa antal personal/ avdelning \n" +
                    "3. Lägg till personal \n" +
                    "4. Ta bort personal \n" +
                    "\n0. Tillbaka <-");
                int userInput;
                while (!int.TryParse(Console.ReadLine(), out userInput))
                {
                    ErrorMessage();
                    Console.ReadKey();
                    return;
                }
                switch (userInput)
                {
                    case 0:
                        running = false;
                        break;
                    case 1:
                        Functions.PrintStaff(context);
                        break;
                    case 2:
                        Functions.AddStaff(context);
                        break;
                    case 3:
                        Functions.RemoveStaff(context);
                        break;
                    default:
                        ErrorMessage();
                        Console.ReadKey();
                        break;
                }
            }
        }
        public static void MainMenu()
        {
            bool running = true;
            while (running)
            {
                using (var context = new Labb4Context())
                {

                    Console.Clear();
                    Console.WriteLine("===|| Meny ||===\n");
                    Console.WriteLine("" +
                        "1. Hämta alla elever \n" +
                        "2. Hämta alla elever från en viss klass \n" +
                        "3. Personal \n" +
                        "4. Personal per avdelning\n" +
                        "\n0. Avsluta");

                    int userInput;
                    while (!int.TryParse(Console.ReadLine(), out userInput) || userInput < 0 || userInput > 4)
                    {
                        UI.ErrorMessage();
                    }
                    switch (userInput)
                    {
                        case 1:
                            Functions.PrintAllStudents(context);
                            break;
                        case 2:
                            Functions.PrintClasses(context);
                            break;
                        case 3:
                            StaffUI(context);
                            break;
                        case 4:
                            Functions.TeachersBySection(context);
                            break;
                        case 0:
                            ExitMessage();
                            running = false;
                            break;
                    }
                }
            }

        }
    }
}
