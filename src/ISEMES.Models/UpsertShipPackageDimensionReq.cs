namespace ISEMES.Models
{
    public class UpsertShipPackageDimensionReq
    {
        public string ShipmentID { get; set; }
        public string LoginID { get; set; }
        public List<Package> Packages { get; set; }
    }

    public class Package
    {
        public string PackageId { get; set; }
        public int PackageNo { get; set; }
        public string CIPackageDimentions { get; set; }
        public int CIWeight { get; set; }
        public bool Active { get; set; }
    }

    public class ShipmentPackage
    {
        public int PackageId { get; set; }
        public int ShipmentId { get; set; }
        public int PackageNo { get; set; }
        public string CIPackageDimentions { get; set; }
        public int CIWeight { get; set; }
        public int CreatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public bool Active { get; set; }
        public int? LastModifiedBy { get; set; }
        public DateTime? LastModifiedOn { get; set; }
    }
}

