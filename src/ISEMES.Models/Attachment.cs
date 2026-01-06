namespace ISEMES.Models
{
    public class Attachment
    {
        public int? AttachmentId { get; set; }
        public int ObjectID { get; set; }
        public string Section { get; set; }
        public string AttachmentName { get; set; }
        public string Path { get; set; }
        public bool Active { get; set; }
        public int LoginId { get; set; }
        public DateTime CreatedON { get; set; }
        public string CreatedBY { get; set; }
    }
    public class AttachmentDto
    {
        public int? AttachmentId { get; set; }
        public string Section { get; set; }
        public string Path { get; set; }
        public bool Active { get; set; }
    }
}



