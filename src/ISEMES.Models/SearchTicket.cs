namespace ISEMES.Models
{
    public class SearchTicket
    {
        public string ID { get; set; }
        public int TicketID { get; set; }
        public int TicketTypeID { get; set; }
        public string TicketType { get; set; }
        public int RequestorID { get; set; }
        public string Requestor { get; set; }
        public string RequestDetails { get; set; }
        public string ASLotString { get; set; }
        public string TicketStatus { get; set; }
        public int TicketStatusID { get; set; }
        public string AcceptedTime { get; set; }
        public string CreatedOn { get; set; }
        public string DueDate { get; set; }
    }

    public class TicketType
    {
        public int TicketTypeID { get; set; }
        public string TicketTypeName { get; set; } = string.Empty;
        public Boolean MultiSelect { get; set; }
    }

    public class TicketLot
    {
        public int InventoryID { get; set; }
        public string LotNum { get; set; }
    }

    public class TicketLotDetail
    {
        public int InventoryID { get; set; }
        public string CustomerName { get; set; }
        public string CustomerLot { get; set; }
        public string DeviceType { get; set; }
        public string ISELot { get; set; }
        public int Qty { get; set; }
        public string ICRLocation { get; set; }
        public string ScanLotNum { get; set; }
    }

    public class Ticket
    {
        public int? TicketID { get; set; }
        public int TicketTypeID { get; set; }
        public string TicketType { get; set; }
        public int RequestorID { get; set; }
        public string RequestDetails { get; set; }
        public string LotString { get; set; }
        public string TicketStatus { get; set; }
        public string DueDate { get; set; }
        public int UserID { get; set; }
        public int Active { get; set; }
        public int StatusID { get; set; }
        public string RecordStatus { get; set; }
    }

    public class TicketDetail : Ticket
    {
        public List<TicketComments> Comments { get; set; }  
        public List<TicketLotDetail> LotDetails { get; set; }
        public List<CommentAttachment> CommentsAttachments { get; set; }
        public List<TicketAttachment> TicketAttachments { get; set; }
    }

    public class TicketAttachment
    {
        public string FileName { get; set; }
        public bool Active { get; set; }
    }

    public class TicketComments
    {
        public int CommentId { get; set; }
        public string ReviewerComments { get; set; }
        public string RequestorComments { get; set; }
        public string CreatedOn { get; set; }
        public string CreatedBy { get; set; }
    }

    public class CommentAttachment
    {
        public int? CommentId { get; set; }
        public string FileName { get; set; }
    }

    public class ScanLot
    {
        public int InventoryID { get; set; }
        public string LotNum { get; set; }
    }
}

