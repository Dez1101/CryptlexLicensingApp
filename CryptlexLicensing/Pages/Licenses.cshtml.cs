using CryptlexLicensingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CryptlexLicensingApp.Pages
{
    public class LicensesModel : PageModel
    {
        public List<License> Licenses { get; set; } = [];
        public void OnGet()
        {
            Licenses = new List<License>
            {
                new License {
                    Id = 1,
                    Name = "Laptop",
                    Type = "Example1",
                    Status = "Example1",
                    ExpiresAt = new DateTime(2024, 05, 11).ToString("yyyy MMMM dd"),
                    ActivatedAt = new DateTime(2024, 05, 11).ToString("yyyy MMMM dd"),
                    DeactivatedAt = new DateTime(2024, 05, 11).ToString("yyyy MMMM dd"),
                }
            };
        }
    }
}
