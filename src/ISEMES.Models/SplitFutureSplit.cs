using System.Xml.Serialization;

namespace ISEMES.Models
{
    public class SplitFutureSplit
    {
    }

    [XmlRoot("SplitStep")]
    public class SplitStepDto
    {
        [XmlElement("lstSplits")]
        public LstSplitsDto LstSplits { get; set; }

        [XmlElement("TrvStepId")]
        public int TrvStepId { get; set; }

        [XmlElement("LotId")]
        public int LotId { get; set; }

        [XmlElement("FSId")]
        public int FSId { get; set; }

        [XmlElement("Source")]
        public string Source { get; set; }

        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("SplitId")]
        public int SplitId { get; set; }
    }

    public class LstSplitsDto
    {
        [XmlElement("TotalSplit")]
        public List<TotalSplitDto> TotalSplits { get; set; }
    }

    public class TotalSplitDto
    {
        [XmlElement("splitId")]
        public int SplitId { get; set; }

        [XmlElement("SplitNo")]
        public int SplitNo { get; set; }

        [XmlElement("isCopySteps")]
        public bool IsCopySteps { get; set; }

        [XmlElement("isDeleted")]
        public bool IsDeleted { get; set; }

        [XmlElement("bins")]
        public BinsDto Bins { get; set; }

        [XmlElement("SourceId")]
        public int SourceId { get; set; }

        [XmlElement("IsCopyShippingInfo")]
        public bool IsCopyShippingInfo { get; set; }
    }

    public class BinsDto
    {
        [XmlElement("SplitBins")]
        public List<SplitBinDto> SplitBins { get; set; }
    }

    public class SplitBinDto
    {
        [XmlElement("BinId")]
        public int BinId { get; set; }

        [XmlElement("SplitQty")]
        public long SplitQty { get; set; }

        [XmlElement("Condition")]
        public string Condition { get; set; }

        [XmlElement("IsFullQty")]
        public bool IsFullQty { get; set; }

        [XmlElement("FSId")]
        public int FSId { get; set; }

        [XmlElement("Id")]
        public int Id { get; set; }

        [XmlElement("IsEnabled")]
        public bool IsEnabled { get; set; }
    }
}

