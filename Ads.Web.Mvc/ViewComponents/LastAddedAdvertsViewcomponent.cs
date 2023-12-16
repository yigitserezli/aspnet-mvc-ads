using App.Business;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.ViewComponents
{
    [ViewComponent(Name = "LastAddedAdverts")]
    public class LastAddedAdvertsViewcomponent : ViewComponent
    {
        readonly IAdvertService _advertService;
        public LastAddedAdvertsViewcomponent(IAdvertService advertService)
        {
            _advertService = advertService;

        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var advertList = _advertService.GetLastTenAdverts().Result.Data;



            return View(advertList);
        }
    
    }

    
}
