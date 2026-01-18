using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ISEMES.Models
{
    public class CheckInCheckOutLotDetails
    {
        public int? RequestId { get; set; }
        public string? RequestedBy { get; set; }
        public DateTime RequestedOn { get; set; }
        public int? CRId { get; set; }
        public int? LotId { get; set; }
        public string? ReceivingNo { get; set; }
        public string? CustomerName { get; set; }
        public string? ISELotNumber { get; set; }
        public int? TotalCount { get; set; }
        public int? AvailableCount { get; set; }
        public int? RunningCount { get; set; }
        public string? GoodsType { get; set; }
        public string? State { get; set; }
        public string? AreaCode { get; set; }
        public string? LocationName { get; set; }
        public bool? CanCheckIn { get; set; }
        public bool? CanEditCheckIn { get; set; }
        public bool? Hold { get; set; }
        public string? CurrentLocation { get; set; }
        public string? CustomerLotNumber { get; set; }
        public string? Device { get; set; }
        public int? Quantity { get; set; }
        public int? LocationId { get; set; }
        public int? LotRequestId { get; set; }
    }
}
