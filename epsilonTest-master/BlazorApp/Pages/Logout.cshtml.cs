using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace BlazorApp.Pages
{
    public class LogoutModel : PageModel
    {
        public async Task<IActionResult> OnGetAsync()
        {
            return new SignOutResult(new[] { "Cookies", "oidc" });
            return SignOut(new[] { "Cookies", "oidc" });
            //await HttpContext.SignOutAsync();
            //return Redirect("/");
            await HttpContext.SignOutAsync("Cookies");
            await HttpContext.SignOutAsync("oidc", AuthProps());
            return Redirect("/");
        }
        private AuthenticationProperties AuthProps()
        => new AuthenticationProperties
        {
            RedirectUri = Url.Content("~/")
        };
    }
}
