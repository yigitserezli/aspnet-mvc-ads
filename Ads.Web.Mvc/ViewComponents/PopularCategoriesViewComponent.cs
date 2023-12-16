using App.Business;
using App.Models.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.ViewComponents
{

    [ViewComponent(Name = "PopularCategories")]
    public class PopularCategoriesViewComponent : ViewComponent
    {
        readonly ICategoryService _categoryService;
        public  PopularCategoriesViewComponent(ICategoryService categoryService)
        {
          _categoryService = categoryService;

        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            ICollection<PopularCategoriesDTO> ListPopularCategories = new List<PopularCategoriesDTO>();
             ListPopularCategories = _categoryService.GetPopularCategories().Result.Data;
            return View(ListPopularCategories);
        }
    }
}
