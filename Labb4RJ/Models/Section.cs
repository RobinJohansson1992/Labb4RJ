using System;
using System.Collections.Generic;

namespace Labb4RJ.Models;

public partial class Section
{
    public int SectionId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Staff> Staff { get; set; } = new List<Staff>();
}
