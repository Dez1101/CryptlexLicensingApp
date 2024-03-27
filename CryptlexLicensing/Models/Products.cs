namespace CryptlexLicensingApp.Models
{
    public class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int TotalLicenses { get; set; }
        public int TotalTrialActivations { get; set; }
        public int TotalProductVersions { get; set; }
        public DateOnly CreationDate { get; set; }

    }
}
