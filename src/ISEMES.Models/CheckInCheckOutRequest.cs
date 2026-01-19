using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISEMES.Models
{
    public class CheckInCheckOutRequest
    {
        public required string RequestType { get; set; }
        public required string InputJson { get; set; }
    }
}
