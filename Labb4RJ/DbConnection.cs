using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Labb4RJ
{
    internal class DbConnection
    {
        public static string GetConnectionString()
        {
            return "Server = localhost; Database = Labb4; Integrated security = true; Trust Server Certificate = true";
        }
    }
}
