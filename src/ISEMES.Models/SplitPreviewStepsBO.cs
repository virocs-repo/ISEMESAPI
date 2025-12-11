namespace ISEMES.Models
{
    public class SplitPreviewStepsBO
    {
        public string? LotNumber { get; set; }
        public decimal Sequence { get; set; }
        public string? Stepname { get; set; }
        public string? QtyIn { get; set; }
        public string? QtyOut { get; set; }
        public string? QualifiedRejectQty { get; set; }
        public string? RejectQty { get; set; }
        public string? ProcessedQty { get; set; }
        public string? RemainingQty { get; set; }
        public string? TimeIn { get; set; }
        public string? TimeOut { get; set; }
        public string? Description { get; set; }
        public string? Source { get; set; }
        public string? Status { get; set; }
        public string? GoodQty { get; set; }
    }
}

