using Labb4RJ.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4RJ
{
    internal class SubjectMethods
    {
        // Method that shows all active subjects (subjects with students):
        public static void ActiveSubjects()
        {
            using (var context = new Labb4Context())
            {
                Console.Clear();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("===|| Kurser ||===\n");
                Console.ResetColor();

                var allSubjects = context.Subjects
                     .GroupJoin(
                      context.Grades,
                      s => s.SubjectId,
                      g => g.SubjectId,
                       (s, grades) => new
                       {
                           s.SubjectId,
                           s.SubjectName,
                           StudentCount = grades.Count()
                       });
                foreach (var a in allSubjects)
                {
                    if (a.StudentCount > 0)
                    {
                        Console.Write($"\n{a.SubjectId}. {a.SubjectName} ({a.StudentCount} elever) - ");
                        Console.ForegroundColor = ConsoleColor.DarkGreen;
                        Console.WriteLine("AKTIV");
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.Write($"\n{a.SubjectId}. {a.SubjectName} ({a.StudentCount} elever) - ");
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("INAKTIV");
                        Console.ResetColor();

                    }
                }
                UIMessages.BackMessage();
            }
        }
    }
}
