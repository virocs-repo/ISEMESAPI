namespace ISEMES.Models
{
    public class MasterList
    {
        public int MasterListItemId { get; set; }
        public int MasterListId { get; set; }
        public string ItemText { get; set; } = string.Empty;
        public string ItemValue { get; set; } = string.Empty;
        public int ParentId { get; set; }
        public int Parent { get; set; }
        public int ParentValue { get; set; }
        public int CreatedBy { get; set; }
        public bool IsDefault { get; set; }
    }
}

