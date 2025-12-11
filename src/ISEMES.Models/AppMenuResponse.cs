namespace ISEMES.Models
{
    public class AppMenuResponse
    {
        public List<AppMenu> AppMenus { get; set; }
        public List<AppFeature> AppFeatures { get; set; }
        public List<AppFeatureField> AppFeatureFields { get; set; }
        public List<MainMenu> MainMenuItem { get; set; }
    }

    public class AppMenu
    {
        public int AppMenuID { get; set; }
        public string MenuTitle { get; set; }
        public string NavigationUrl { get; set; }
        public string Description { get; set; }
        public int AppFeatureID { get; set; }
        public int? ParentID { get; set; }
        public int AppMenuIndex { get; set; }
        public int SequenceNumber { get; set; }
        public bool Active { get; set; }
    }

    public class AppFeature
    {
        public int AppMenuId { get; set; }
        public int AppFeatureId { get; set; }
        public int FeatureID { get; set; }
        public string FeatureName { get; set; }
        public bool Active { get; set; }
    }

    public class AppFeatureField
    {
        public int AppFeatureID { get; set; }
        public int FeatureFieldId { get; set; }
        public string FeatureName { get; set; }
        public string FeatureFieldName { get; set; }
        public bool Active { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsWriteOnly { get; set; }
    }

    public class MainMenu
    {
        public int LoginId { get; set; }
        public string NavigationUrl { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Facility { get; set; } = string.Empty;
        public string RoleName { get; set; } = string.Empty;
    }
}

