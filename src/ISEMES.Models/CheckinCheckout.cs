using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class InventoryCheckinCheckout
    {
        [Key]
        public int InventoryId {  get; set; }
        public string LotNum { get; set; }
        public string Location { get; set; }
        public string ReceivedFromId { get; set; }
        public string ReceivedFrom { get; set; }
        public string Person { get; set; }
        public int Qty { get; set; }
        public int CheckinCheckOutQTY { get; set; }
        public string SystemUser { get; set; }
        public int InventoryStatusId { get; set; }
        public string InventoryStatus { get; set; }
        public string GoodsType { get; set; }
        public string Area { get; set; }
        public string WIPLocation { get; set; }
        public string CustomerName {  get; set; }
        public string Device {  get; set; }
        public string ReceivedFromCheckOut { get; set; }
        public DateTime? CheckInOutTime { get; set; }
        public string SystemCheckInOutPerson { get; set; }
        public string CurrentLocation { get; set; }  
        public string CustomerLotNumber { get; set; }
    }

    public class InventoryCheckinCheckoutItem
    {
        public int InventoryId { get; set; }
        public string LotNum { get; set; }
        public int LocationId { get; set; }
        public string Location { get; set; }
        public string ReceivedFrom { get; set; }
        public string Person { get; set; }
        public int Qty { get; set; }
        public string SystemUser { get; set; }
        public string Status { get; set; }
        public string ReceivedFromId { get; set; }
        public int InventoryStatusId { get; set; }
        public string GoodsType { get; set; }
    }

    public class InventoryCheckinCheckoutStatuses
    {
        public int InventoryStatusID { get; set; }
        public string InventoryStatus { get; set; }
    }

    public class InventoryCheckinCheckoutLocation
    {
        public int InventoryLocationID { get; set; }
        public string Location { get; set; }
    }

    public class InventoryCheckinCheckoutDetail
    {
        public int? InventoryID { get; set; }
        public string Location { get; set; }
        public int? StatusID { get; set; }
        public string? ReceivedFrom { get; set; }
        public int LoginId { get; set; }
    }

    public class InventoryCheckinCheckoutRequest
    {
        public List<InventoryCheckinCheckoutDetail> InvMovementDetails { get; set; }
    }

    public class CheckinCheckoutStatus
    {
        public string InventoryStatus { get; set; }
    }
}



