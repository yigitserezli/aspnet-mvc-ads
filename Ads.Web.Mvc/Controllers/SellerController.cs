using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using App.Data;
using App.Data.Entities;
using App.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;
using AutoMapper;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Configuration.UserSecrets;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{

    public class SellerController : Controller
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;
        public SellerController(AppDbContext dbContext, IMapper mapper)

        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

       

        public int userId => int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));


        // GET: Advert/Index myads




        [HttpGet]
        public IActionResult Index(bool? confirm = null)
        {
          // user id cookiden gelecek
            IQueryable<AdvertEntity> advertsQuery = _dbContext.Adverts.Where(c => c.UserId == userId);

            if (confirm==false)
            {
                advertsQuery = advertsQuery.Where(c => c.Confirm == false);
            }
            if (confirm==true)
            {
                advertsQuery = advertsQuery.Where(c => c.Confirm == true);
            }

            var MyAdverts = advertsQuery.ToList();

            return View("index",MyAdverts);
        }
        public IActionResult MyAdDetail([FromRoute] int id)
        {
            var MyAdvert = _dbContext.Adverts
         .Include(a => a.AdvertComments)
         .Include(a => a.CustomerFavList) 
         .FirstOrDefault(a => a.Id == id);


            return View(MyAdvert);
         
        }


        // GET: Advert/Create
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.SelectItem = _dbContext.Categories.
                ToList().Select(x => new SelectListItem()
                {
                    Text = x.Name,
                    Value = x.Id.ToString(),

                });
            ViewBag.ProductCount = _dbContext.Categories.CountAsync();


            return View();
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IFormFile ImageUrl, AdvertEntity model)
        {
            if (ImageUrl != null && ImageUrl.Length > 0)
            {
                var Advert = new AdvertEntity
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CreatedAt = DateTime.UtcNow,             
                    StockCount = model.StockCount,
                    Confirm = false,
                    CategoryId = model.CategoryId,
                    UserId = userId,
                    ImageUrl = "",

                };

                _dbContext.Adverts.Add(Advert);
                await _dbContext.SaveChangesAsync();

                Advert.ImageUrl = "adv" + Advert.Id + Path.GetExtension(ImageUrl.FileName);
                var fullFilePath = Path.Combine(GetUploadFolder(), Advert.ImageUrl);

                using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await ImageUrl.CopyToAsync(fileStream);
                }

                _dbContext.Entry(Advert).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }

            return RedirectToAction("Index");
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existingAdvert = await _dbContext.Adverts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == userId); //cookie

            ViewBag.SelectItem = _dbContext.Categories.
           ToList().Select(x => new SelectListItem()
           {
               Text = x.Name,
               Value = x.Id.ToString(),

           });
            ViewBag.ProductCount = _dbContext.Categories.CountAsync();


            return View(existingAdvert);
        }


        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] IFormFile ImageUrl, AdvertEntity model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var existingAdvert = await _dbContext.Adverts.FindAsync(id);

      //      if (ModelState.IsValid)
        //    {
                existingAdvert.Name = model.Name;
                existingAdvert.Description = model.Description;
                existingAdvert.Price = model.Price;
                existingAdvert.UpdatedAt = DateTime.UtcNow;
                existingAdvert.StockCount = model.StockCount;
                existingAdvert.CategoryId = model.CategoryId;
                existingAdvert.Confirm = false; // satıcının her güncellemesinde false çekilir ancak admin için bu işlem yapılmaz

               if (ImageUrl != null && ImageUrl.Length > 0)
                {
                    var oldImagePath = Path.Combine(GetUploadFolder(), existingAdvert.ImageUrl);
                    if (!string.IsNullOrEmpty(existingAdvert.ImageUrl) && System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    existingAdvert.ImageUrl = "adv" + existingAdvert.Id + Path.GetExtension(ImageUrl.FileName);
                    var fullFilePath = Path.Combine(GetUploadFolder(), existingAdvert.ImageUrl);

                    using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        await ImageUrl.CopyToAsync(fileStream);
                    }
              }

                _dbContext.Entry(existingAdvert).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();

                return RedirectToAction("Index");
       //     }

        //    return View(model);
        }

        //dosya yükleme işlemi için gerektli metodler
        private string GetUploadFolder()
        {
            var uploadsFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", "adverts");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            return uploadsFolder;
        }
    }
}
