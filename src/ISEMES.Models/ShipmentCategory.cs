using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class ShipmentCategory
    {
        [Key]
        public int ShipmentCategoryID { get; set; }
        public string ShipmentCategoryName { get; set; }
    }
}



