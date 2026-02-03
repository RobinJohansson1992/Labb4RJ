using Labb4RJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4RJ
{
    internal class StudentMethods
    {
        // Method that prints all students by class:
        public static void PrintStudentsByClass(Labb4Context context, List<Class> allClasses)
        {
            bool running = true;

            while (running)
            {
                int userInput;
                if (!int.TryParse(Console.ReadLine(), out userInput))
                {
                    UI.ErrorMessage();
                    Console.ReadKey();
                    continue;
                }
                if (userInput == 0)
                {
                    running = false;
                    continue;
                }
                if (userInput < 1 || userInput > allClasses.Count)
                {
                    UI.ErrorMessage();
                    Console.ReadKey();
                    continue;
                }
                Console.Clear();
                var students = context.Students
                    .Where(s => s.ClassId == userInput) // only print the students in chosen class
                    .ToList();
                foreach (var s in students)
                {
                    Console.WriteLine($"{s.FirstName} {s.LastName}");
                }

                Console.WriteLine("\nTryck enter för att gå tillbaka.");
                Console.ReadLine();
                running = false;
            }
        }
        // Method that prints all students in the school:
        public static void PrintAllStudents(Labb4Context context)
        {
            Console.Clear();

            var allStudents = context.Students
                .Join(context.Classes,
                    s => s.ClassId,
                    c => c.ClassId,
                    (s, c) => new
                    {
                        Class = c,
                        Student = s
                    })
                .OrderBy(x => x.Student.StudentId)
                .ToList();

            foreach (var x in allStudents)
            {
                Console.WriteLine(
                    $"{x.Student.StudentId}. {x.Student.FirstName} {x.Student.LastName}, Personnummer: {x.Student.PersonNumber} Klass: {x.Class.ClassName}"
                );
            }
            Console.WriteLine("\nTryck enter för att återgå <-");
            Console.ReadKey();
        }

        

    }
}
