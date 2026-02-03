using System;
using System.Collections.Generic;

namespace Labb4RJ.Models;

public partial class Grade
{
    public int GradeId { get; set; }

    public int TeacherId { get; set; }

    public int SubjectId { get; set; }

    public int StudentId { get; set; }

    public string Grade1 { get; set; } = null!;

    public DateOnly GradeDate { get; set; }

    public virtual Student Student { get; set; } = null!;

    public virtual Subject Subject { get; set; } = null!;

    public virtual Staff Teacher { get; set; } = null!;
}
