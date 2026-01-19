using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISEMES.Models
{
    public class EmployeeDetails
    {
        public int EmployeeId { get; set; }
        public int CustomerLoginId { get; set; }
        public required string UserName { get; set; }
    }
}
