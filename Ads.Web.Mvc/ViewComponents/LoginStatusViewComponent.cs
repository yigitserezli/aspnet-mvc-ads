using Ads.Web.Mvc.Models;
using App.Data.Entities;
using App.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.ViewComponents
{

    [ViewComponent(Name = "LoginStatus")]
    public class LoginStatusViewComponent : ViewComponent
    {


        public LoginStatusViewComponent()
        {

        }

        public async Task<IViewComponentResult> InvokeAsync(LoginViewModel model)
        {
             



                return View(model);
            
        }

    }
}
