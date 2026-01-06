using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class HardwareDetails
    {
        [Key]
        public int HardwareID { get; set; }
        public int ReceiptID { get; set; }
        public int InventoryID { get; set; }
        public string? CustomerName { get; set; }
        public string? CustomerHardwareID { get; set; }        
        public int HardwareTypeID { get; set; }
        public string? HardwareType { get; set; }               
        public string? SerialNumber { get; set; }
        public int ExpectedQty { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
        public Boolean IsReceived { get; set; }
    }

    public class HardwareRequest
    {        
        public int? HardwareID { get; set; }
        public int? ReceiptID { get; set; }
        public string CustomerHardwareID { get; set; }
        public int? HardwareTypeID { get; set; }
        public int? ExpectedQty { get; set; }
        public string? RecordStatus { get; set; }
        public bool? Active { get; set; }
        public int? LoginId { get; set; }        
        public Boolean IsReceived { get; set; }
    }

    public class HardwareDetailsRequest
    {
        public List<HardwareRequest> HardwareDetails { get; set; }
    }

    public class HardwareTypeDetails
    {
        [Key]
        public int HardwareTypeID { get; set; }
        public string? HardwareType { get; set; }
    }
}



