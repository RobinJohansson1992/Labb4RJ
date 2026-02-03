using Labb4RJ.Models;
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
                UI.ErrorMessage();
            }

            var staffToRemove = context.Staff.First(s => s.StaffId == staffId);

            context.Staff.Remove(staffToRemove);
            context.SaveChanges();

            Console.WriteLine($"{staffToRemove.Name} togs bort från anställd-listan.");
            Console.ReadKey();
        }
        // Method that lets the user add staff to the staff-table:
        public static void AddStaff(Labb4Context context)
        {
            Console.Clear();
            var roles = context.Roles
               .OrderBy(r => r.RoleId)
               .ToList();
            Console.WriteLine("Tillgängliga roller:");
            foreach (var role in roles)
            {
                Console.WriteLine($"{role.RoleId}. {role.RoleName}");
            }
            Console.WriteLine();
            Console.Write("Ange roll-ID för ny anställd: ");
            int roleId;
            while (!int.TryParse(Console.ReadLine(), out roleId) || roleId > roles.Count || roleId < 1)
            {
                UI.ErrorMessage();
            }
            Console.Write("Ange namn för ny anställd: ");
            string name = Console.ReadLine();

            //Create new staff-member:
            var newStaff = new Staff
            {
                Name = name,
                RoleId = roleId,
            };
            context.Staff.Add(newStaff); //Add member to the table
            context.SaveChanges(); //save to DB
            Console.WriteLine($"{newStaff.Name} lades till i anställd-listan.");
            Console.ReadKey();
        }
        // Method that prints all staff members in chosen order by user:
        public static void PrintStaff(Labb4Context context)
        {
            Console.Clear();
            // Join Staff and Role to get needed data:
            var allStaff = context.Staff
                .Join(
                context.Roles,
                s => s.RoleId,
                r => r.RoleId,
                (s, r) => new
                {
                    Roles = r,
                    Staff = s
                })
                .OrderBy(s => s.Staff.StaffId).ToList();
            foreach (var a in allStaff)
            {
                Console.WriteLine($"{a.Staff.StaffId}. {a.Staff.Name}: {a.Roles.RoleName}");
            }
            Console.ReadKey();
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
            foreach (var item in result)
            {
                Console.WriteLine();
                Console.WriteLine($"{item.SectionName} - {item.StaffCount} anställda");
            }
            Console.WriteLine("\nTryck enter för att återgå <-");
            Console.ReadKey();
        }
    }
}
