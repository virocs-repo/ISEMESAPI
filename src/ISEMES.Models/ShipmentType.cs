using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class ShipmentType
    {
        [Key]
        public int ShipmentTypeID { get; set; }
        public string ShipmentTypeName { get; set; }
    }
}



