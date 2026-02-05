using Labb4RJ.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Labb4RJ
{
    internal class StudentMethods
    {
        // Method that shows info about chosen student by ID:
        public static void StudentInfoById()
        {
            Console.Clear();
            Console.WriteLine("Ange student-ID för elev:");
            Console.Write("\nStudent-ID: ");

            int userInput = UI.CheckInput();
            var connectionString = DbConnection.GetConnectionString();
            string query = "studentInfo";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@StudentId", userInput);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string firstName = reader.GetString(0);
                            string lastName = reader.GetString(1);
                            string personNumber = reader.GetString(2);
                            string className = reader.GetString(3);
                            string teacherName = reader.GetString(4);


                            Console.Clear();
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.WriteLine($"{firstName} {lastName}s info: \n");
                            Console.ResetColor();


                            Console.WriteLine($"Personnummer: {personNumber}");
                            Console.WriteLine($"Klass: {className}");
                            Console.WriteLine($"Lärare: {teacherName}");
                        }
                        else
                        {
                            Console.WriteLine($"Ingen elev hittades med ID {userInput}");
                        }
                    }
                    connection.Close();
                }
            }
            UI.BackToMainMessage();
            Console.ReadLine();
        }
        // Method that shows grades for each student:
        public static void GradesByStudent()
        {
            Console.Clear();
            Console.WriteLine("Ange student-ID för elev:");
            Console.Write("\nStudent-ID: ");
            int userInput = UI.CheckInput();

            // Get connection string from Connection class:
            var connectionString = DbConnection.GetConnectionString();
            string query = @"
                     SELECT s.StudentId, s.FirstName, s.LastName, su.SubjectName,
                       g.Grade, g.GradeDate, st.Name
                         FROM Students s
                         JOIN Grades g ON g.StudentId = s.StudentId
                          JOIN Subjects su ON su.SubjectId = g.SubjectId
                           JOIN Staff st ON st.StaffId = g.TeacherId
                              WHERE st.RoleID = 1 AND s.StudentId = @StudentId";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    command.Parameters.AddWithValue("@StudentId", userInput);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        bool header = false;

                        while (reader.Read())
                        {
                            int studentId = reader.GetInt32(0);
                            string firstName = reader.GetString(1);
                            string lastName = reader.GetString(2);
                            string subjectName = reader.GetString(3);
                            string grade = reader.GetString(4);
                            DateTime date = reader.GetDateTime(5);
                            string teacherName = reader.GetString(6);
                            if (!header)
                            {
                                Console.Clear();
                                Console.ForegroundColor = ConsoleColor.DarkYellow;
                                Console.WriteLine($"{studentId}. {firstName} {lastName}s betyg: \n");
                                Console.ResetColor();
                                header = true;
                            }
                            Console.WriteLine($"{subjectName}: [{grade}] || {date:dd MMM yyyy} || av: {teacherName}\n");
                        }
                    }
                    connection.Close();
                }
            }
            UI.BackToMainMessage();
            Console.ReadLine();
        }
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
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("\nTryck enter för att gå tillbaka.");
                Console.ResetColor();
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
            UI.BackToMainMessage();
            Console.ReadKey();
        }
        // Method that prints all classes:
        public static void PrintClasses(Labb4Context context)
        {
            Console.Clear();
            UI.PrintClassesUI();
            var allClasses = context.Classes
                .OrderBy(c => c.ClassId)
                .ToList();
            foreach (var c in allClasses)
            {
                Console.WriteLine($"{c.ClassId}. {c.ClassName}");
            }
            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine($"\n0. Tillbaka <-");
            Console.ResetColor();
            StudentMethods.PrintStudentsByClass(context, allClasses);
        }



    }
}
