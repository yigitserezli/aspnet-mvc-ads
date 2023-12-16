using App.Business;
using App.Data.Entities;
using App.Models.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.ViewComponents
{

    [ViewComponent(Name = "CategoryMenu")]
    public class CategoryMenuViewComponent : ViewComponent
    {
        readonly ICategoryService _categoryService;

        public CategoryMenuViewComponent(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

            ICollection<CategoryEntity> parentCategoryList = new List<CategoryEntity>();
            parentCategoryList = _categoryService.GetAllCategories().Result.Data;
            return View(parentCategoryList);

        }

    }
}
