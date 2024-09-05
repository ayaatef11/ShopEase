// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using Microsoft.AspNetCore.Mvc.RazorPages;

namespace myshop.Web.Areas.Identity.Pages.Account
{
    public class AccessDeniedModel : PageModel
    {
        string ReturnUrl { get; set; }  
        public void OnGet(string returnUrl = null)
        {
            // Optional: Set the return URL if available, or set to a default page
            ReturnUrl = returnUrl ?? "/";
        }
    }
}
