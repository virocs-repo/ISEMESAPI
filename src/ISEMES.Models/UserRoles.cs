namespace ISEMES.Models
{
    public class UserRoles
    {
        public List<MainMenuItem> MainMenuItem { get; set; }
    }
    public class Feature
    {
        public int AppFeatureId { get; set; }
        public int FeatureID { get; set; }
        public string FeatureName { get; set; }
        public bool Active { get; set; }
        public List<FeatureField> FeatureField { get; set; }
    }
    public class FeatureField
    {
        public int AppFeatureID { get; set; }
        public int FeatureFieldId { get; set; }
        public string FeatureFieldName { get; set; }
        public bool Active { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsWriteOnly { get; set; }
        public bool ShowField { get; set; }
    }
    public class MainMenuItem
    {
        public int AppMenuID { get; set; }
        public string MenuTitle { get; set; }
        public string NavigationUrl { get; set; }
        public string Description { get; set; }
        public int AppFeature { get; set; }
        public int ParentID { get; set; }
        public int AppMenuIndex { get; set; }
        public int SequenceNumber { get; set; }
        public bool Active { get; set; }
        public List<Feature> Feature { get; set; }
        public int LoginId { get; set; }
        public string UserName { get; set; }
    }
}



