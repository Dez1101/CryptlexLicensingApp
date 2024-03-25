namespace CryptlexLicensingApp.Models
{
    public class ProductModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int TotalLicenses { get; set; }
        public int TotalTrialActivations { get; set; }
        public int TotalProductVersions { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
