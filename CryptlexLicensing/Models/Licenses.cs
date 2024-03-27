namespace CryptlexLicensingApp.Models
{
    public class License
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string Status { get; set; }
        public string ExpiresAt { get; set; }
        public string ActivatedAt { get; set; }
        public string DeactivatedAt { get; set; }
    }
}
