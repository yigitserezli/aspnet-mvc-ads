using App.Business;
using App.Data.Entities;
using App.Models.CategoryDTOs;
using Microsoft.AspNetCore.Mvc;

namespace Ads.Web.Mvc.ViewComponents
{
    [ViewComponent(Name ="HomePageAllCategories")]
    public class HomePageAllCategoriesViewcomponent : ViewComponent
    {
        readonly ICategoryService _categoryService;
        readonly IAdvertService _advert;
        public HomePageAllCategoriesViewcomponent(ICategoryService categoryService,  IAdvertService advert)
        {
            _categoryService = categoryService;
            _advert = advert;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {

           
            ICollection<HomePageCategoryListDTO> homePageCategoryList =new List<HomePageCategoryListDTO>();

            ICollection<CategoryEntity> CategoryList = new List<CategoryEntity>();
            ICollection<CategoryEntity> parentCategoryList = new List<CategoryEntity>();

            CategoryList = _categoryService.GetAllCategories().Result.Data;

            parentCategoryList = CategoryList.Where(x => x.parentCategoryID == null).ToList();



            foreach(var item in parentCategoryList)
            {
                HomePageCategoryListDTO homePageCategoryListItem = new HomePageCategoryListDTO();
                homePageCategoryListItem.categoryName = item.Name;

                List<CategoryEntity> subCategories = new List<CategoryEntity>();
                subCategories = CategoryList.Where(x => x.parentCategoryID == item.Id).Take(4).ToList();

                if (subCategories.Count != 0)
                {
                    foreach (var subCategory in subCategories)
                    {
                        SubCategory subCategoryItem = new SubCategory();
                        subCategoryItem.AdvertCount = _advert.GetAllAdvertsUnderGivenCategory(item.Id).Result.Data.Count() ;
                        subCategoryItem.Name = subCategory.Name;
                        homePageCategoryListItem.subCategories.Add(subCategoryItem);

                    }
                }
                homePageCategoryList.Add(homePageCategoryListItem);

            }
            
            return View(homePageCategoryList);

        }
    }
}
