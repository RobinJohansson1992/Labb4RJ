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
        public static void ActiveSubjects(Labb4Context context)
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("===|| Aktiva kurser ||===\n");
            Console.ResetColor();

            var allSubjects = context.Subjects
                .Join(context.Grades,
                s => s.SubjectId,
                g => g.SubjectId,
                (s,g) => new
                {
                    s.SubjectId, s.SubjectName, g.StudentId
                })
                .GroupBy(x => new {x.SubjectId, x.SubjectName})
                .Select(g => new
                {
                    g.Key.SubjectId,
                    g.Key.SubjectName,
                    StudentCount = g.Count()
                });
            foreach (var a in allSubjects)
            {
                Console.WriteLine($"\n{a.SubjectId}. {a.SubjectName} ({a.StudentCount} elever)");
            }
            UI.BackToMainMessage();
            Console.ReadKey();

        }
    }
}
