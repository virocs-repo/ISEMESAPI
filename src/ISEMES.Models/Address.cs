using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class Address
    {
        [Key]        
        public int AddressId { get; set; }
        public string? AddressType { get; set; }
        public string? Address1 { get; set; }
        public string? Address2 { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Phone { get; set; }
        public string? ShipTo { get; set; }
        public int? AEType { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime ModifiedOn { get; set; }
        public bool Active { get; set; }
    }
}



