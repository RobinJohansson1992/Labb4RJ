using System;
using System.Collections.Generic;

namespace Labb4RJ.Models;

public partial class Student
{
    public int StudentId { get; set; }

    public int ClassId { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string PersonNumber { get; set; } = null!;

    public virtual Class Class { get; set; } = null!;

    public virtual ICollection<Grade> Grades { get; set; } = new List<Grade>();
}
