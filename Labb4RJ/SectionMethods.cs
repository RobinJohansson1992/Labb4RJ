using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4RJ
{
    internal class SectionMethods
    {
        public static void AverageSalaryBySection()
        {
            Console.Clear();
            var connectionString = DbConnection.GetConnectionString();

            string query = "SELECT s.Name, AVG(st.MonthlySalary) FROM Section s " +
                "JOIN Staff st ON s.SectionId = st.SectionId GROUP BY s.Name";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Meddellön per månad (avdelning):\n");
                Console.ResetColor();

                using (var command = new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string section = reader.GetString(0);
                            decimal averageSalary = reader.GetDecimal(1);
                            Console.WriteLine($"{section}: {averageSalary:F2} kr.\n");
                        }
                    }
                }
                connection.Close();
            }
            UI.BackToMainMessage();
            Console.ReadKey();
        }
        public static void TotalSalaryBySection()
        {
            Console.Clear();
            var connectionString = DbConnection.GetConnectionString();

            string query = "SELECT s.Name, SUM(st.MonthlySalary) AS totalLön FROM Staff st " +
                "JOIN Section s ON st.SectionId = s.SectionId GROUP BY s.Name";

            using(var connection = new SqlConnection(connectionString))
            {
                connection.Open();
                Console.ForegroundColor = ConsoleColor.DarkYellow;
                Console.WriteLine("Total lön per månad (avdelning):\n");
                Console.ResetColor();

                using (var command= new SqlCommand(query, connection))
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string section = reader.GetString(0);
                            decimal totalSalary = reader.GetDecimal(1);
                            Console.WriteLine($"{section}: {totalSalary} kr.\n");
                        }
                    }
                }
                connection.Close();
            }
            UI.BackToMainMessage();
            Console.ReadKey();
        }
    }
}
