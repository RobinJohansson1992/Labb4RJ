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
        public static void RemoveStaff(Labb4Context context)
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
                    catch (Exception)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("Ogiltig inmatning");
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
            string query = "SELECT s.Name, r.RoleName, s.YearHired FROM Staff s JOIN Roles r ON s.RoleId = r.RoleId";

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
                            string name = reader.GetString(0);
                            string role = reader.GetString(1);
                            int yearHired = reader.GetInt32(2);

                            Console.WriteLine($"{name} : {role}, anställd sedan {yearHired}");
                        }
                    }
                }
            }
            UIMessages.BackMessage();
        }

        public static void TeachersBySection(Labb4Context context)
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
