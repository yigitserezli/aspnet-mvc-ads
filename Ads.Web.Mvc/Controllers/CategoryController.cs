using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using App.Business;
using App.Data;
using App.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{
    
    public class CategoryController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly ICategoryService category;
        public CategoryController(ICategoryService _categoryService, AppDbContext dbContext)
        {
            _dbContext = dbContext;
            category = _categoryService;
        }
        // GET: /<controller>/
        public IActionResult Index([FromRoute]int? id)
        {
            List<AdvertEntity> AdvertList = new List<AdvertEntity>();

            if (id != 0 && id !=  null)
            {
                AdvertList = _dbContext.Adverts.Where(x => x.CategoryId == id).Include(cat => cat.Category).ToList();
            }
            else
            {
                AdvertList = _dbContext.Adverts.ToList();
            }
            if (AdvertList != null)
            {
                return View(AdvertList);
            }

            return NotFound();
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

