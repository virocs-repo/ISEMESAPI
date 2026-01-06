using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class MiscellaneousGoods
    {
        [Key]
        public int MiscellaneousGoodsID { get; set; }
        public int ReceiptID { get; set; }
        public int InventoryID { get; set; }
        public int CustomerVendorID { get; set; }
        public string CustomerVendor { get; set; }
        public string SerialNumber { get; set; }
        public string AdditionalInfo { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
        public Boolean IsReceived { get; set; }
    }

    public class MiscellaneousGoodsDetails
    {
        public int? MiscellaneousGoodsID { get; set; }
        public int ReceiptID { get; set; }        
        public string AdditionalInfo { get; set; }        
        public string RecordStatus { get; set; }
        public bool Active { get; set; }
        public int LoginId { get; set; }
        public Boolean IsReceived { get; set; }
    }

    public class MiscellaneousGoodsDetailsRequest
    {
        public List<MiscellaneousGoodsDetails> MiscGoodsDetails { get; set; }
    }
}



