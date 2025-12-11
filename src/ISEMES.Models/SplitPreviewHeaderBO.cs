namespace ISEMES.Models
{
    public class SplitPreviewHeaderBO
    {
        public string? LotNumber { get; set; }
        public string? Customer { get; set; }
        public string? DeviceFamily { get; set; }
        public string? Device { get; set; }
        public string? Category { get; set; }
        public string? DQPId { get; set; }
        public string? TotalQty { get; set; }
        public string? RunningQty { get; set; }
        public string? CurrentQty { get; set; }
        public List<SplitPreviewStepsBO>? PreviewSteps { get; set; }
    }
}

