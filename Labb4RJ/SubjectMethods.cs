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
        // Method that shows all active subjects:
        public static void ActiveSubjects(Labb4Context context)
        {
            Console.Clear();
            Console.WriteLine("===|| Aktiva kurser ||===\n");

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
