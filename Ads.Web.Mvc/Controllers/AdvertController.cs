using App.Data.Entities;
using App.Data;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using App.Business;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;




// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{
    public class AdvertController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IAdvertService _advertService;

        public AdvertController(AppDbContext dbContext, IAdvertService advertService)

        {
            _dbContext = dbContext;
            _advertService = advertService;

        }
        // GET: /<controller>/


        public int userId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

        // GET: Advert/Index
        [Authorize(Roles = "Admin,User")]
        
        public IActionResult Index()
        {
            var adverts = _dbContext.Adverts.Where(a => a.Confirm == true).ToList();
            return View(adverts);
        }

        // GET: Advert/Detail/5
        public IActionResult AdDetail(int id)
        {
            var advert = _dbContext.Adverts
           .Include(a => a.AdvertComments.Where(ac => ac.Confirm))
           .Include(a => a.CustomerFavList)
           .FirstOrDefault(a => a.Id == id && a.Confirm==true);

            if (advert != null)
            {
                return View(advert);
            }

            return NotFound();
        }

      
        // MY FAVORITES
        public IActionResult MyFavorites()
        {

            var MyFavorites = _dbContext.CustomerFavLists
                         .Where(a => a.UserId == userId)
                         .Include(fav => fav.Advert) 
                         .ToList();

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

        [HttpGet]
        public IActionResult AddAdvert()
        {



            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddAdvert([FromForm]IFormFile ImageUrl, AdvertEntity advert, int userId)
        {
            int user = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            var test = await _advertService.AddAdvertToDb(ImageUrl, advert, user);


            return View();
        }

        public IActionResult PlaceOrder(int advertId, int quantity)
        {
            var advert = _dbContext.Adverts.FirstOrDefault(a => a.Id == advertId);

            if (advert == null)
            {
                return NotFound("Advert not found!");
            }

            if (quantity > advert.StockCount)
            {
                return BadRequest("Ordered quantity exceeds available stock!");
            }
                       
            var oldStockCount = advert.StockCount;
            advert.StockCount -= quantity;
            var orderPrice = advert.Price * quantity;
          
            var existingOrder = _dbContext.Orders.FirstOrDefault(o => o.AdvertId == advertId);

            if (existingOrder != null)
            {
                existingOrder.Quantity += quantity;
                _dbContext.Orders.Update(existingOrder);
            }
            else
            {
               
                var newOrder = new OrdersEntity
                {
                    AdvertId = advertId,
                    UserId = userId,
                    Quantity = quantity,
                    Price = orderPrice, 
                    OrderDate = DateTime.UtcNow,
                };

                _dbContext.Orders.Add(newOrder);
            }

            _dbContext.SaveChanges();

            return Ok();
        }

        public IActionResult MyOrders()
        {
            var adverts = _dbContext.Orders
                .Where(a => a.UserId == userId)
                .Include(a =>a.Advert)
                .ToList();
            return View(adverts);
        }



    }
}
