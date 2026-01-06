namespace ISEMES.Models
{
    public class InventoryMoveStatus
    {
        public int InventoryID { get; set; }
        public string LotNum { get; set; }
        public int? LocationID { get; set; }
        public string Location { get; set; }
        public int? ReceivedFromID { get; set; }
        public string ReceivedFrom { get; set; }
        public string Person { get; set; }
        public int Qty { get; set; }
        public int CheckInOutQty { get; set; }
        public string SystemUser { get; set; }
        public int InventoryStatusID { get; set; }
        public string InventoryStatus { get; set; }
        public string GoodsType { get; set; }
        public DateTime ModifiedOn { get; set; }
        public string Area { get; set; }
        public string WIPLocation { get; set; }
        public string CustomerName { get; set; }
        public string Device { get; set; }
        public string ReceivedFromOrCheckOut { get; set; }
        public DateTime CheckInOutTime { get; set; }
        public string SystemCheckInOutPerson { get; set; }
        public string CurrentLocation { get; set; }
        public string FacilityArea { get; set; }
        public string Facility {  get; set; }
        public string CustomerLotNumber { get; set; }
    }
}



