namespace ISEMES.Models
{
    public class AreaByFacility
    {
        public int Area_FacilityId { get; set; }
        public int AreaID { get; set; }
        public string Area_Name { get; set; }
        public int FacilityId { get; set; }
    }

    public class LotInfoAreaByFacility
    {
        public int InventoryID { get; set; }
        public string CurrentLocation { get; set; }
        public int? Area_FacilityID { get; set; }
        public int? FacilityID { get; set; }
    }

    public class InventoryMoveRequest
    {
        public int InventoryId { get; set; }
        public int? AreaFacilityId { get; set; }
        public int? FacilityId { get; set; }
    }
}

