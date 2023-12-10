using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Threading.Tasks;
using App.Data;
using App.Data.Entities;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.EntityFrameworkCore;


// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Ads.Web.Mvc.Controllers
{
    public class SellerController : Controller
    {
        private readonly AppDbContext _dbContext;
        public SellerController(AppDbContext dbContext)
   
        {
            _dbContext = dbContext;
        
        }
     



        // GET: Advert/Index myads
       

        [HttpGet]
        public IActionResult Index()
        {
         var MyAdverts = _dbContext.Adverts.Where(c => c.UserId == 1).ToList();
           
            return View(MyAdverts);

        }

        [HttpGet]
        public IActionResult MyAdDetail(int advertId)
        {
            var advert = _dbContext.Adverts
                .FirstOrDefault(a => a.Id == advertId);

            if (advert != null)
            {
                return View(advert);
            }

            return NotFound();
        }

        // GET: Advert/Create
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] IFormFile file, AdvertEntity model)
        {
            if (ModelState.IsValid)
            {
                var Advert = new AdvertEntity
                {
                    Name = model.Name,
                    Description = model.Description,
                    Price = model.Price,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow,
                    StockCount = model.StockCount,
                    Confirm = false,
                    CategoryId = model.CategoryId,
                    UserId = model.UserId //buradaki id yi cookie bilgisinden alması gerekiyor
                    //Imageurl ilana imaj yüklenip yüklenmemesine göre aşağıda ilanın ıd numarası ile ekleniyor
                    
                };

                if (file != null && file.Length > 0)
                {
                    _dbContext.Adverts.Add(Advert);
                    await _dbContext.SaveChangesAsync();

                    Advert.ImageUrl = "adv" + Advert.Id;
                    var fullFilePath = Path.Combine(GetUploadFolder(), Advert.ImageUrl);

                    using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    Advert.ImageUrl = ""; 
                    _dbContext.Adverts.Add(Advert);
                    await _dbContext.SaveChangesAsync();
                }

              

                return RedirectToAction("Index");
            }

            return View(model);
        }



        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var existingAdvert = await _dbContext.Adverts.FirstOrDefaultAsync(a => a.Id == id && a.UserId == 1); //cookie

            return View(existingAdvert);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] IFormFile file, AdvertEntity model)
        {
            if (id != model.Id)
            {
                return NotFound();
            }

            var existingAdvert = await _dbContext.Adverts.FindAsync(id);

            if (ModelState.IsValid)
            {
                existingAdvert.Name = model.Name;
                existingAdvert.Description = model.Description;
                existingAdvert.Price = model.Price;
                existingAdvert.UpdatedAt = DateTime.UtcNow;
                existingAdvert.StockCount = model.StockCount;
                existingAdvert.CategoryId = model.CategoryId;
                existingAdvert.Confirm = false; // satıcının her güncellemesinde false çekilir ancak admin için bu işlem yapılmaz

                if (file != null && file.Length > 0)
                {
                    var oldImagePath = Path.Combine(GetUploadFolder(), existingAdvert.ImageUrl);
                    if (!string.IsNullOrEmpty(existingAdvert.ImageUrl) && System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }

                    _dbContext.Entry(existingAdvert).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();

                    existingAdvert.ImageUrl = "adv" + existingAdvert.Id;
                    var fullFilePath = Path.Combine(GetUploadFolder(), existingAdvert.ImageUrl);

                    using (var fileStream = new FileStream(fullFilePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                }
                else
                {
                    _dbContext.Entry(existingAdvert).State = EntityState.Modified;
                    await _dbContext.SaveChangesAsync();
                }

                return RedirectToAction("Index");
            }

            return View(model);
        }













        //dosya yükleme işlemi için gerektli metodler


        private string GetMimeType(string fileName)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(fileName, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            return contentType;
        }

        private string GetUploadFolder()
        {
            var folderPath = Path.Combine(Environment.CurrentDirectory, "uploads");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            return folderPath;
        }
    }
}
