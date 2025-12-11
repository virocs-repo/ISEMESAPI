using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class ShipmentLineItem
    {
        [Key]
        public int InventoryID { get; set; }        
        public int? ShipmentLineItemID { get; set; }        
        public string? CustomerLotNum { get; set; }
        public string ISELotNum { get; set; }
        public string GoodsType { get; set; }
        public string? PartNum { get; set; }
        public int CurrentQty { get; set; }        
        public int ShipmentQty { get; set; }
        public int? ShipmentTypeID { get; set; }
        public string? ShipmentType { get; set; }
        public string? Address { get; set; }
        public string? AdditionalInfo { get; set; }
        public string? ToStreet1 { get; set; }
        public string? ToCity { get; set; }
        public string? ToState { get; set; }
        public string? ToZip { get; set; }
        public string? ToCountry { get; set; }
        public string? ToName { get; set; }
        public string? ToPhone { get; set; }
        public int? DeliveryInfoid { get; set; }
    }

    public class ShipmentInventory
    {
        [Key]
        public int InventoryID { get; set; }        
        public string? CustomerLotNum { get; set; }
        public string ISELotNum { get; set; }
        public string GoodsType { get; set; }
        public string? PartNum { get; set; }
        public int CurrentQty { get; set; }
        public int ShipmentQty { get; set; }
        public int? ShipmentTypeID { get; set; }
        public string? ShipmentType { get; set; }
        public string? Address { get; set; }        
    }
}

