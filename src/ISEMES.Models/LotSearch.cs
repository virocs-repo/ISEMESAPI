namespace ISEMES.Models
{
    public class LotSearch
    {
        public int LotId { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string ISELotNumber { get; set; } = string.Empty;
        public string DeviceFamily { get; set; } = string.Empty;
        public string Device { get; set; } = string.Empty;
        public string TravelerStatus { get; set; } = string.Empty;
        public string LotStatus { get; set; } = string.Empty;
        public string CustomerLotNumber { get; set; } = string.Empty;
        public int? ExpectedCount { get; set; }
        public int? RunningCount { get; set; }
        public int? LotQty { get; set; }
        public string ExpectedDate { get; set; } = string.Empty;
        public string Identifier { get; set; } = string.Empty;
        public string ReceivingNo { get; set; } = string.Empty;
        public string ShippingMethod { get; set; } = string.Empty;
        public string SOStatus { get; set; } = string.Empty;
        public string CurrentStep { get; set; } = string.Empty;
        public string NextStep { get; set; } = string.Empty;
        public string CurrentLocation { get; set; } = string.Empty;
    }

    public class LotDetail
    {
        public string? ISELotNumber { get; set; }
        public int? CustomerId { get; set; }
        public bool? IsPackage { get; set; }
        public string? DeviceFamilyId { get; set; }
        public int? DeviceId { get; set; }
        public int? DeviceAliasId { get; set; }
        public int? OurCount { get; set; }
        public int? CurrentCount { get; set; }
        public int RunningCount { get; set; }
        public int? ShipLocation { get; set; }
        public bool? IQANotRequired { get; set; }
        public int? DeviceTypeId { get; set; }
        public int? COOId { get; set; }
        public string? DateCode { get; set; }
        public string? PartType { get; set; }
        public string? CustomerLotNumber { get; set; }
        public int? ServiceCategoryId { get; set; }
        public int? AssemblyId { get; set; }
        public string? ContactInfo { get; set; }
        public string? Trays { get; set; }
        public int? LotMissingQty { get; set; }
        public bool? UnitsOnReel { get; set; }
        public bool? ShipToScrap { get; set; }
        public bool? Expedite { get; set; }
        public bool? IsClosed { get; set; }
        public int? BomId { get; set; }
        public int? ISEOwnerId { get; set; }
        public int? DataCategoryId { get; set; }
        public string? Identifier { get; set; }
        public List<TRVStep> TRVSteps { get; set; } = new List<TRVStep>();
    }

    public class TRVStep
    {
        public int ID { get; set; }
        public int LotId { get; set; }
        public int TRVStepId { get; set; }
        public int? ParentTRVStepId { get; set; }
        public string LotNum { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string StepName { get; set; } = string.Empty;
        public int? ServiceId { get; set; }
        public int? ParentLotSplitStepServiceId { get; set; }
        public string ParentSplitService { get; set; }
        public int? ParentServiceId { get; set; }
        public int? SubServiceId { get; set; }
        public int? SourceId { get; set; }
        public string Source { get; set; } = string.Empty;
        public int? SourceReferenceId { get; set; }
        public int? SetupProgramId { get; set; }
        public string SetupSiteType { get; set; } = string.Empty;
        public string AlertComments { get; set; } = string.Empty;
        public string StepType { get; set; } = string.Empty;
        public string DeviationCode { get; set; } = string.Empty;
        public string DeviationComments { get; set; } = string.Empty;
        public decimal Sequence { get; set; }
        public int SequenceGroup { get; set; }
        public decimal StepSequence { get; set; }
        public int TotalQty { get; set; }
        public int? StepTotalQty { get; set; }
        public int? ProcessedQty { get; set; }
        public int? RemainingQty { get; set; }
        public int? MissingQty { get; set; }
        public int? StepRemainingQty { get; set; }
        public int? GoodQty { get; set; }
        public int? RejectQty { get; set; }
        public int? QualifiedRejectQty { get; set; }
        public string? StartTime { get; set; }
        public string? EndTime { get; set; }
        public  string? Status { get; set; }
        public decimal? OverAllYield { get; set; }
        public decimal? ActualYeild { get; set; }
        public int? IspartialInvoice { get; set; }
        public string? BatchNo { get; set; }
        public string? BatchInvoiceStatus { get; set; }
        public int? ShowFS { get; set; }
        public int? EditFS { get; set; }
        public int? ShowMerge { get; set; }
        public int? SplitAvailable { get; set; }
        public int? EditSplit { get; set; }
    }

    public class MergeLot
    {
        public int LotId { get; set; }
        public int DeviceId { get; set; }
        public bool IsPackage { get; set; }
        public string CustomerLotNumber { get; set; }
        public string ISELotNumber { get; set; }
        public int ISEOwnerId { get; set; }
        public int RunningCount { get; set; }
        public bool IsReceived { get; set; }
        public int TravelerStatusId { get; set; }
        public string TravelerStatus { get; set; }
        public int StepCount { get; set; }
        public string Steps { get; set; }
    }

    public class IcrSearch
    {
        public int LotId { get; set; }
        public string ControlRoomLocation { get; set; }
        public string ISELotNumber { get; set; }
        public string CustomerName { get; set; }
        public string Requestor { get; set; }
        public string RequestType { get; set; }
        public string LotStatus { get; set; }
        public string TravelerStatus { get; set; }
        public string Status { get; set; }
        public string BinBox { get; set; }
        public int? NoOfBinbox { get; set; }
        public string CurrentLocation { get; set; }
        public string CurrentLotLocation { get; set; }
        public DateTime RequestedOn { get; set; }
        public string PreviousStep { get; set; }
        public int TRVStepId { get; set; }
    }

    public class MergedLots
    {
        public int MergeId { get; set; }
        public int TRVStepId { get; set; }
        public int LotId { get; set; }
        public string MergedLotIds { get; set; }
        public string MergedLotNumbers { get; set; }
        public bool IsCompleted { get; set; }
        public bool Active { get; set; }
    }

    public class LotMerge
    {
        public int LotId { get; set; }
        public int MergeLotIdStr { get; set; }
        public int UserId { get; set; }
        public bool IsConsolidateSplit { get; set; }
    }

    public class MergeRequestDto
    {
        public int? MergeId { get; set; }
        public int TrvStepId { get; set; }
        public string? LotIds { get; set; }
        public int UserId { get; set; }
    }

    public class FutureSplitBin
    {
        public int Id { get; set; }
        public string? BinId { get; set; }
        public string? BinCode { get; set; }
        public string? Condition { get; set; }
        public string? Description { get; set; }
    }

    public class SplitBin
    {
        public int Id { get; set; }
        public string? BinId { get; set; }
        public string? Qty { get; set; }
        public string? BinCode { get; set; }
        public string? Condition { get; set; }
        public string? Description { get; set; }
        public bool HasOtherRejects { get; set; }
        public bool SplitTotalRejects { get; set; }
        public string? DisplayBinCode { get; set; }
    }

    public class FutureSplit
    {
        public int FSId { get; set; }
        public int TRVStepId { get; set; }
        public int SplitNo { get; set; }
        public List<TotalSplitDto> TotalSplits { get; set; }
        public bool Active { get; set; }
        public string Source { get; set; }
        public int LotId { get; set; }
        public int? SourceId { get; set; }
    }

    public class Split
    {
        public int SplitId { get; set; }
        public int TRVStepId { get; set; }
        public int SplitNo { get; set; }
        public string SplitXML { get; set; }
        public bool Active { get; set; }
        public int LotId { get; set; }
    }
}

