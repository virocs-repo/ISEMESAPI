using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISEMES.Models
{
    public class CheckInCheckOutLocation
    {
        public string? AreaCode { get; set; }
        public int LocationId { get; set; }
        public string? LocationName { get; set; }
        public int LotId { get; set; }
    }
}
