
#nullable disable


//using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.WebUtilities;

namespace myshop.Web.Areas.Identity.Pages.Account
{
    public class RegisterModel(
            UserManager<IdentityUser> _userManager,
            IUserStore<IdentityUser> _userStore,
            SignInManager<IdentityUser> _signInManager,
            ILogger<RegisterModel> _logger,
            IEmailSender _emailSender,
            RoleManager<IdentityRole> _roleManager) : PageModel
    {
       //field initializer or thas attributes can't be assigned here but int he on post function they could be 
        private  IUserEmailStore<ApplicationUser> _emailStore;
     
        [BindProperty]
        public InputModel Input { get; set; }
        public string ReturnUrl { get; set; }
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public class InputModel
        {

            [Required]
            public string Name { get; set; }
            public string Address { get; set; }
            public string City { get; set; }

            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }
            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }
            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
        }


        public async Task OnGetAsync(string returnUrl = null)
        {
            if (!_roleManager.RoleExistsAsync(SD.AdminRole).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.AdminRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.EditorRole)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.CustomerRole)).GetAwaiter().GetResult();
            }
            
            ReturnUrl = returnUrl;
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();/*Calls _signInManager.GetExternalAuthenticationSchemesAsync(): This method, part of the SignInManager class in ASP.NET Core Identity, retrieves a list of external authentication schemes configured for the application. These schemes represent third-party login providers, such as Google, Facebook, Microsoft, Twitter, or any other OAuth-based providers that have been set up in the application.

                                                                                                         Awaits the Result: The method returns a task, so await is used to asynchronously wait for the task to complete and retrieve the result. This ensures that the application does not block while waiting for the operation to finish, making the process non-blocking and more efficient.
                                                                                                         
                                                                                                         Converts to a List: The ToList() method converts the resulting collection of authentication schemes into a List<AuthenticationScheme>. This step makes it easier to work with the collection in the code, such as passing it to views or further processing.
                                                                                                         
                                                                                                         Assigns to ExternalLogins: The result, a list of external authentication schemes, is assigned to the ExternalLogins property. This property is typically used in the UI to display available external login options to the user.*/
         if (ModelState.IsValid)
            {
                var user = CreateUser();
                //_emailStore = GetEmailStore();
                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                //await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                user.City = Input.City;
                user.Name = Input.Name;
                user.Address = Input.Address;
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");


                    string role = HttpContext.Request.Form["RoleRadio"].ToString();

                    if (String.IsNullOrEmpty(role))
                    {
                        await _userManager.AddToRoleAsync(user, SD.CustomerRole);
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        //return LocalRedirect(returnUrl);
                    }
                    else
                    {
                        await _userManager.AddToRoleAsync(user, role);
                    }
                     var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));//encode it 
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                        pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    else
                    {
                        
                    }
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return RedirectToAction("Index", "Users", new { area = "Admin" });
            // If we got this far, something failed, redisplay form
           // return Page();
        }

        private ApplicationUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<ApplicationUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        public IUserEmailStore<ApplicationUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<ApplicationUser>)_userStore;
        }
    }
}
