using System.ComponentModel.DataAnnotations;

namespace ISEMES.Models
{
    public class ReceiptEmployee
    {
        [Key]
        public int ReceiptxEmployeeID { get; set; }
        public int ReceiptID { get; set; }
        public int EmployeeID { get; set; }
        public string Employee { get; set; }
        public string Email { get; set; }
    }
}



