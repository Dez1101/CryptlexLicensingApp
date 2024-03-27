using CryptlexLicensingApp.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace CryptlexLicensingApp.Pages
{
    [Authorize]
    public class ProductsModel : PageModel
    {
        public List<Product> Products { get; set; } = [];

        public void OnGet()
        {
            // Populate the list with some dummy products
            Products = new List<Product>
            {
                new Product {
                    Id = 1,
                    Name = "Laptop",
                    TotalLicenses = 4,
                    TotalTrialActivations = 0,
                    CreationDate = new DateOnly(2024, 03, 28)
                },
                new Product {
                    Id = 1,
                    Name = "Laptop",
                    TotalLicenses = 4,
                    TotalTrialActivations = 0,
                    CreationDate = new DateOnly(2024, 03, 28)
                },
                new Product {
                    Id = 1,
                    Name = "Laptop",
                    TotalLicenses = 4,
                    TotalTrialActivations = 0,
                    CreationDate = new DateOnly(2024, 03, 28)
                }
            };
        }
    }
}
