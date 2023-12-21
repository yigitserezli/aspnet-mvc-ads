
using App.Data.Entities;
using App.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;



// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{
    public class AdvertController : Controller
    {
        private readonly AppDbContext _dbContext;
        public AdvertController(AppDbContext dbContext)

        {
            _dbContext = dbContext;

        }
        // GET: /<controller>/


        public int userId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // GET: Advert/Index
        public IActionResult Index()
        {
            var adverts = _dbContext.Adverts.Where(a => a.Confirm == true).ToList();
            return View(adverts);
        }

        // GET: Advert/Detail/5
        public IActionResult AdDetail(int id)
        {
            var advert = _dbContext.Adverts
                  .Include(a => a.AdvertComments)
                  .FirstOrDefault(a => a.Confirm == true);

            if (advert != null)
            {
                return View(advert);
            }

            return NotFound();
        }


        // MY FAVORITES
        public IActionResult MyFavorites()
        {
        
            var MyFavorites = _dbContext.CustomerFavLists.Where(a => a.UserId == userId).ToList();
            return View(MyFavorites);
        }

        public IActionResult Favorite(int id)
        {
            var existingFavList = _dbContext.CustomerFavLists.FirstOrDefault(f => f.UserId == userId && f.AdvertId == id);

            if (existingFavList != null)
            {
                _dbContext.CustomerFavLists.Remove(existingFavList);
                _dbContext.SaveChanges();
                return Ok("Favorite removed");
            }

            var newFavorite = new CustomerFavListentity
            {
                UserId = userId,
                AdvertId = id
            };

            _dbContext.CustomerFavLists.Add(newFavorite);
            _dbContext.SaveChanges();

            return Ok("Favorite added");
        }




    }
}
