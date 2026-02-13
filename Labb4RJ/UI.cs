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
        // UI for GradeStudentWithId-method:
        public static void GradeStudentWithIdUI()
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Sätt betyg för elev:\n");
            Console.ResetColor();

            Console.Write("\nStudent-ID: ");
            int studentId = UIMessages.CheckInput();
            Console.Write("\nÄmnes-ID: ");
            int subjectId = UIMessages.CheckInput();
            Console.Write("\nLärar-ID: ");
            int teacherId = UIMessages.CheckInput();
            string grade = string.Empty;
            bool gradeInput = false;
            while (!gradeInput)
            {
                Console.Write("\nBetyg (A–F): ");
                grade = Console.ReadLine()?.Trim().ToUpper();

                if (!string.IsNullOrEmpty(grade) && grade.Length == 1 && grade[0] >= 'A' && grade[0] <= 'F')
                {
                    gradeInput = true;
                }
                else
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Felaktigt betyg. Ange en bokstav mellan A och F.");
                    Console.ResetColor();
                }
            }
            Console.WriteLine();
            StudentMethods.GradeStudentWithId(studentId, subjectId, teacherId, grade);
            UIMessages.BackMessage();
        }
        public static void PrintClassesUI()
        {
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Välj klass från listan:\n");
            Console.ResetColor();
        }

        // Under-menu: Students:
        public static void StudentsUI()
        {
            using (var context = new Labb4Context())
            {

                bool running = true;
                while (running)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("===|| Elever ||===\n");
                    Console.ResetColor();
                    Console.WriteLine("" +
                        "1. Elevinfo \n" +
                        "2. Elever per klass \n" +
                        "3. Visa betyg \n" +
                        "4. Sätt betyg på elev\n" +
                        "5. Hämta viktig info om elev med elev-ID\n");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("0. Tillbaka <-");
                    Console.ResetColor();

                    int userInput;
                    while (!int.TryParse(Console.ReadLine(), out userInput) || userInput < 0 || userInput > 5)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Du måste ange ett nummer från listan.");
                        Console.ResetColor();
                    }
                    switch (userInput)
                    {
                        case 1:
                            StudentMethods.PrintAllStudents();
                            break;
                        case 2:
                            StudentMethods.PrintClasses();
                            break;
                        case 3:
                            StudentMethods.GradesByStudent();
                            break;
                        case 4:
                            GradeStudentWithIdUI();
                            break;
                        case 5:
                            StudentMethods.StudentInfoById();
                            break;
                        case 0:
                            running = false;
                            return;
                    }
                }
            }
        }
        // Under-menu: Staff:
        public static void StaffUI()
        {
                bool running = true;
                while (running)
                {
                    Console.Clear();
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("===|| Personal ||===\n");
                    Console.ResetColor();
                    Console.WriteLine("" +
                        "1. Visa personal \n" +
                        "2. Visa antal personal/ avdelning \n" +
                        "3. Lägg till personal \n" +
                        "4. Ta bort personal \n");
                    Console.ForegroundColor = ConsoleColor.DarkYellow;
                    Console.WriteLine("0. Tillbaka <-");
                    Console.ResetColor();
                    int userInput;
                    while (!int.TryParse(Console.ReadLine(), out userInput))
                    {
                        UIMessages.ErrorMessage();
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
                            break;
                        case 2:
                            StaffMethods.TeachersBySection();
                            break;
                        case 3:
                            AddStaffUI();
                            break;
                        case 4:
                            StaffMethods.RemoveStaff();
                            break;
                        default:
                            UIMessages.ErrorMessage();
                            Console.ReadKey();
                            break;
                    }
            }
        }
        // UI for AddStaff-method:
        public static void AddStaffUI()
        {
            using (var context = new Labb4Context())
            {
                Console.Clear();
                var roles = context.Roles
                   .OrderBy(r => r.RoleId)
                   .ToList();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Tillgängliga roller:\n");
                Console.ResetColor();
                foreach (var role in roles)
                {
                    Console.WriteLine($"{role.RoleId}. {role.RoleName}");
                }
                Console.WriteLine("\nLägg till information om ny anställd:\n");
                Console.Write("Namn: ");
                string name = Console.ReadLine();
                Console.Write("Roll-ID: ");
                int roleId = UIMessages.CheckInput();
                Console.Write("Avdelnings-ID: ");
                int sectionId = UIMessages.CheckInput();
                Console.Write("Anställdes år: ");
                int yearHired = UIMessages.CheckInput();
                Console.Write("Månadslön: ");
                int monthlySalary = UIMessages.CheckInput();

                StaffMethods.AddStaff(name, roleId, sectionId, yearHired, monthlySalary);

                UIMessages.BackMessage();
                Console.ReadKey();
            }
        }
        // Under-menu: Section:
        public static void SectionMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("===|| Avdelningar ||===\n");
                Console.ResetColor();
                Console.WriteLine("" +
                    "1. Total löneutbetalning / avdelning \n" +
                    "2. Medellön / avdelning \n");
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("0. Tillbaka <-");
                Console.ResetColor();
                int userInput;
                while (!int.TryParse(Console.ReadLine(), out userInput) || userInput < 0 || userInput > 2)
                {
                    UIMessages.ErrorMessage();
                }
                switch (userInput)
                {
                    case 1:
                        SectionMethods.TotalSalaryBySection();
                        break;
                    case 2:
                        SectionMethods.AverageSalaryBySection();
                        break;
                    case 0:
                        running = false;
                        break;
                }
            }
        }
        // Main-menu:
        public static void MainMenu()
        {
            bool running = true;
            while (running)
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("===|| Huvudmeny ||===\n");
                Console.ResetColor();
                Console.WriteLine("" +
                    "1. Elever \n" +
                    "2. Personal \n" +
                    "3. Kurser \n" +
                    "4. Avdelningar \n");
                Console.ForegroundColor = ConsoleColor.DarkRed;
                Console.WriteLine("0. Avsluta");
                Console.ResetColor();

                int userInput;
                while (!int.TryParse(Console.ReadLine(), out userInput) || userInput < 0 || userInput > 4)
                {
                    UIMessages.ErrorMessage();
                }
                switch (userInput)
                {
                    case 1:
                        StudentsUI();
                        break;
                    case 2:
                        StaffUI();
                        break;
                    case 3:
                        SubjectMethods.ActiveSubjects();
                        break;
                    case 4:
                        SectionMenu();
                        break;
                    case 0:
                        UIMessages.ExitMessage();
                        running = false;
                        break;
                }
            }

        }
    }
}
