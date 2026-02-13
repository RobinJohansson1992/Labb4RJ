using Azure;
using Labb4RJ.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4RJ
{
    internal class StaffMethods
    {
        // Method that removes chosen staffmember by user:
        public static void RemoveStaff()
        {
            using (var context = new Labb4Context())
            {
                Console.Clear();
                var allStaff = context.Staff
                    .Join(
                    context.Roles,
                    s => s.RoleId,
                    r => r.RoleId,
                    (s, r) => new
                    {
                        Roles = r,
                        Staff = s
                    }
                    )
                    .OrderBy(s => s.Staff.StaffId)
                    .ToList();
                foreach (var a in allStaff)
                {
                    Console.WriteLine($"{a.Staff.StaffId}. {a.Staff.Name}: {a.Roles.RoleName}");
                }
                Console.Write("\nAnge anställd-ID på den anställda du vill ta bort: ");
                int staffId;
                while (!int.TryParse(Console.ReadLine(), out staffId) || !allStaff.Any(s => s.Staff.StaffId == staffId))
                {
                    UIMessages.ErrorMessage();
                }

                var staffToRemove = context.Staff.First(s => s.StaffId == staffId);

                context.Staff.Remove(staffToRemove);
                context.SaveChanges();

                Console.WriteLine($"{staffToRemove.Name} togs bort från anställd-listan.");
                Console.ReadKey();
            }
        }
        // Method that lets the user add staff to the staff-table:
        public static void AddStaff(string name, int roleId, int sectionId, int yearHired, decimal monthlySalary)
        {
            var connectionString = DbConnection.GetConnectionString();
            using (var connection = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO Staff (name, roleId, sectionId, yearHired, monthlySalary)
                             VALUES (@Name, @RoleId, @SectionId, @YearHired, @MonthlySalary)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    command.Parameters.AddWithValue("@RoleId", roleId);
                    command.Parameters.AddWithValue("@SectionId", sectionId);
                    command.Parameters.AddWithValue("@YearHired", yearHired);
                    command.Parameters.AddWithValue("@MonthlySalary", monthlySalary);

                    // fail-safe try/catch:
                    try
                    {
                        connection.Open();
                        command.ExecuteNonQuery();
                        Console.WriteLine($"\n{name} lades till i anställd-listan.");
                    }
                    catch (Exception ex)
                    {
                        Console.ForegroundColor = ConsoleColor.DarkRed;
                        Console.WriteLine("Ogiltig inmatning.\n");
                        Console.WriteLine($"'{ex.Message}'");
                        Console.ResetColor();
                    }
                    connection.Close();
                }
            }
        }
        // Method that prints all staff with info:
        public static void PrintStaff()
        {
            Console.Clear();
            // Get connection string from Connection class:
            var connectionString = DbConnection.GetConnectionString();
            string query = "SELECT s.staffId, s.Name, r.RoleName, s.YearHired FROM Staff s JOIN Roles r ON s.RoleId = r.RoleId";

            using (var connection = new SqlConnection(connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        Console.ForegroundColor = ConsoleColor.DarkYellow;
                        Console.WriteLine("Anställda:\n");
                        Console.ResetColor();

                        while (reader.Read())
                        {

                            int staffId = reader.GetInt32(0);
                            string name = reader.GetString(1);
                            string role = reader.GetString(2);
                            int yearHired = reader.GetInt32(3);

                            Console.Write($"{staffId}. ");
                            Console.ForegroundColor = ConsoleColor.DarkYellow;
                            Console.Write($"{name}");
                            Console.ResetColor();
                            Console.WriteLine($" - {role}, anställd sedan {yearHired}");
                        }
                    }
                }
            }
            UIMessages.BackMessage();
        }
        // Method that shows how many techers work in each section:
        public static void TeachersBySection()
        {
            using (var context = new Labb4Context())
            {
                Console.Clear();
                var result = context.Staff
                    .GroupBy(s => s.Section)
                    .Select(g => new
                    {
                        SectionName = g.Key.Name,
                        StaffCount = g.Count()
                    })
                    .OrderBy(x => x.SectionName)
                    .ToList();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Antal anställda per avdelning:");
                Console.ResetColor();
                foreach (var item in result)
                {
                    Console.WriteLine();
                    Console.WriteLine($"{item.SectionName} - {item.StaffCount} anställda");
                }

                UIMessages.BackMessage();
            }
        }
    }
}
