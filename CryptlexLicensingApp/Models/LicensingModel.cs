﻿using Cryptlex;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System;

public class LicensingModel : PageModel
{
    [BindProperty]
    public string LicenseKey { get; set; } = string.Empty;

    public string Message { get; set; } = string.Empty;

    public void OnGet()
    {
        try
        {
            string productDat = "QjM4QjdENDA5MUVGM0UwNUMwRDk1MURBMTY2NTBEQzk=.fczOlEdlXaqbFfsVXR3VPZllf951bvgJktAEIAzFD2Zb3/wDBX/nwpd0KWvL34vW2Tc7NrhsOWuAOcxY4VbzH7AoWxdtIMFbBp4aBwtLdaj5y025hzM0Pn7Me9ADyjeSgwBhdTzY3+sK1IrQtKPt7jswFnZ9TG4FIYbYjXVnA6nBNj8fTFubMOI6jI8fvwpbsE/RW/RQ0//dxAInhAoOh2CISczo6KEsQCQRBIlQ+8u582W5X/GWh7Tq2QSHcQqhVPmTh657ZptYW+xbFe4CXQDr2e+7VDqaVPYGVj4Zlv4sDhWQ9VBo9b5jYWkR1GdtKTCgZswGXNaUx1COKwpzNEp4Hvj8y4dhQSRo6M+hGdXLtb82CHsS0veUE8CzqZTzRJCfiJkXM5jzCiUlWNfHBchDXxk79yKN6MEW2dkWYJe87h8V+aC/l3b5AS/D4Ttx0VmREIWlgzOs8lOVoLZrNOuKtaD+w5tCXt9YT4LnfGY3bm3lkpg4Zf+WJd6iYK+gigq7YO1w/Eh2gWaK7D0HhuXw+4DntzDntBk3sIBX39h3mi5cE7qLiNl1ch0fQ+je/NbJL5T0re7MT/YdG7IjzMgdtVif0sYntn1235sVKq9z1VLacq38Cs1CpKfj5XVUlbpCXgUOqHZ/tjs6o0+xF2RCUVxnMd3D1kAoj1oGpLpgMZtskNehjaqsyT9afBZHuiigTwPEmIadT1THyfJu6J1Lpa+0ovU4UFGZy/t6Wxe4g/9qEgm7GAjHIN1a61eqn9VSoopMVkeeZQyzBS0NDRCmKg7FulArYjYIFeNs5j+JXtQd4PWY48hU8xDqbv8e";
            
            LexActivator.SetProductData(productDat);

            LexActivator.SetProductId("4cb82153-a96a-4285-a68a-86221103e05f", LexActivator.PermissionFlags.LA_USER);

        }
        catch (LexActivatorException e)
        {
            Message = $"An error occurred while setting product data: {e.Message}";
        }
    }

    public IActionResult OnPostActivateLicense()
    {
        if (!ModelState.IsValid)
        {
            Message = "Please provide a valid license key.";
            return Page();
        }

        try
        {
            LexActivator.SetLicenseKey(LicenseKey);

            var status = LexActivator.ActivateLicense();
            if (status == LexStatusCodes.LA_OK || status == LexStatusCodes.LA_EXPIRED || status == LexStatusCodes.LA_SUSPENDED)
            {
                Message = "License activated successfully.";
            }
            else
            {
                Message = $"Failed to activate the license. Error code: {status}";
            }
        }
        catch (LexActivatorException e)
        {
            Message = $"An error occurred while activating the license: {e.Message}";
        }

        return Page(); 
    }
}
