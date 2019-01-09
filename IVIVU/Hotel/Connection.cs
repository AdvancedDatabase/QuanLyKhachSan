using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hotel
{
    class Connection
    {
        public static string connectionString()
        {
            return @"Data Source=DESKTOP-O5VB9S0\TUAN;Initial Catalog=QUANLYKHACHSAN;Integrated Security=True;MultipleActiveResultSets=True;Application Name=EntityFramework";
        }
    }
}
