using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{
    
    public class CategoryController : Controller
    {
        private readonly ICategoryService category;
        public CategoryController(ICategoryService _categoryService)
        {
            category = _categoryService;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Search()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetSubcategories(int categoryId)
        {
            var subcategories = category.GetSubCategories(categoryId);
            return Json(subcategories.Result.Data);
        }
        [HttpGet]
        public IActionResult GetParentCategories(int categoryId)
        {
            var subcategories = category.GetParentCategories();
            return Json(subcategories.Result.Data);
        }
    }
}

