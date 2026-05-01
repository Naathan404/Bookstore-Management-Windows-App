namespace Bookstore.API.Models
{
    public class Feature : IEntity
        // CHUCNANG
    {
        public int FeatureID { get; set; }
        public required string FeatureName { get; set; }
        public string ScreenName { get; set; } = string.Empty;
        public int GetID() => FeatureID;
    }
}
