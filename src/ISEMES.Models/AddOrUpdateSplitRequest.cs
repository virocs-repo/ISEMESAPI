using System.Text.Json;
using System.Text.Json.Serialization;

namespace ISEMES.Models
{
    public class AddOrUpdateSplitRequest
    {
        public int Id { get; set; }
        public int FSId { get; set; }
        public bool CopySteps { get; set; }
        public bool CopyShipping { get; set; }
        public bool IsDeleted { get;set; }
        [JsonExtensionData]
        public Dictionary<string, JsonElement> ExtraFields { get; set; }       
    }

    public class SplitRequest
    {
        public int UserId { get; set; }
        public int TrvStepId { get; set; }
        public List<AddOrUpdateSplitRequest> AddOrUpdateSplits { get; set; }
    }
}

