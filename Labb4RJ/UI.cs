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
        public static int CheckInput()
        {
            int userInput;
            while (!int.TryParse(Console.ReadLine(), out userInput))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("Felaktig inmatning.");
                Console.ResetColor();
            }
            return userInput;

        }
        // Students-menu:
        public static void StudentsUI(Labb4Context context)
        {
            Console.Clear();
            Console.WriteLine("Elever:\n");
            Console.WriteLine("" +
                "1. Elevinfo \n" +
                "2. Elever per klass \n" +
                "3. Visa betyg \n" +
                "4. Sätt betyg på elev\n" +
                "5. Hämta viktig info om elev med elev-ID\n" +
                "\n0. Tillbaka <-");

            int userInput;
            while(!int.TryParse(Console.ReadLine(), out userInput) || userInput < 0 || userInput > 5)
            {
                Console.WriteLine("Du måste ange ett nummer från listan.");
            }
            switch (userInput)
            {
                case 1:
                    StudentMethods.PrintAllStudents(context);
                    break;
                case 2:
                    StudentMethods.PrintClasses(context);
                    break;
                case 3:
                    StudentMethods.GradesByStudent(context);
                    break;
                case 4:
                    //make grade
                    break;
                case 5:
                    //important student info
                    break;
                case 0:
                    return;
            }
        }
        public static void ErrorMessage()
        {
            Console.WriteLine("Du måste välja ett nummer från listan.");
        }
        public static void BackToMainMessage()
        {
            Console.WriteLine("\nTryck enter för att gå tillbaka <-");
        }
        public static void ExitMessage()
        {
            Console.WriteLine("\nProgrammet avslutas...");
            Console.ReadKey();
        }
        // Staff-menu:
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
                        return;
                    case 1:
                        StaffMethods.PrintStaff();
                        return;
                    case 2:
                        StaffMethods.TeachersBySection(context);
                        break;
                    case 3:
                        AddStaffUI(context);
                        break;
                    case 4:
                        StaffMethods.RemoveStaff(context);
                        break;
                    default:
                        ErrorMessage();
                        Console.ReadKey();
                        break;
                }
            }
        }
        
        public static void AddStaffUI(Labb4Context context)
        {
            Console.Clear();
            var roles = context.Roles
               .OrderBy(r => r.RoleId)
               .ToList();
            Console.WriteLine("Tillgängliga roller:\n");
            foreach (var role in roles)
            {
                Console.WriteLine($"{role.RoleId}. {role.RoleName}");
            }
            Console.WriteLine("\nLägg till information om ny anställd:\n");
            Console.Write("Namn: "); 
            string name = Console.ReadLine();
            Console.Write("Roll-ID: ");
            int roleId = CheckInput();
            Console.Write("Avdelnings-ID: ");
            int sectionId = CheckInput();
            Console.Write("Anställdes år: ");
            int yearHired = CheckInput();
            Console.Write("Månadslön: ");
            int monthlySalary = CheckInput();

            StaffMethods.AddStaff(name, roleId, sectionId, yearHired, monthlySalary);

            BackToMainMessage();
            Console.ReadKey();
        }
        // Main-menu:
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
                        "1. Elever \n" +
                        "2. Personal \n" +
                        "3. Kurser \n" +
                        "\n0. Avsluta");

                    int userInput;
                    while (!int.TryParse(Console.ReadLine(), out userInput) || userInput < 0 || userInput > 4)
                    {
                        UI.ErrorMessage();
                    }
                    switch (userInput)
                    {
                        case 1:
                            StudentsUI(context);
                            break;
                        case 2:
                            StaffUI(context);
                            break;
                        case 3:
                            SubjectMethods.ActiveSubjects(context);
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
