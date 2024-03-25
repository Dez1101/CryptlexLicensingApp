
public class LicenseRequestModel
{
    public string Os { get; set; }
    public string OsVersion { get; set; }
    public string Fingerprint { get; set; }
    public string VmName { get; set; }
    public bool Container { get; set; }
    public string Hostname { get; set; }
    public string UserHash { get; set; }
    public string ProductId { get; set; }
    public string AccountId { get; set; }
    public string ReleaseVersion { get; set; }
    public string ReleaseChannel { get; set; }
    public string ReleasePlatform { get; set; }
    public DateTime ReleasePublishedAt { get; set; }
    public string Key { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string UserId { get; set; }
    public int LeaseDuration { get; set; }
    public List<Metadata> Metadata { get; set; }
    public List<MeterAttribute> MeterAttributes { get; set; }
}
