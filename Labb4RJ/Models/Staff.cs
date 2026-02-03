using System;
using System.Collections.Generic;

namespace Labb4RJ.Models;

public partial class Staff
{
    public int StaffId { get; set; }

    public string Name { get; set; } = null!;

    public decimal MonthlySalary { get; set; }

    public int YearHired { get; set; }

    public int RoleId { get; set; }

    public int SectionId { get; set; }

    public virtual ICollection<Class> Classes { get; set; } = new List<Class>();

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();

    public virtual Role Role { get; set; } = null!;

    public virtual Section Section { get; set; } = null!;
}
