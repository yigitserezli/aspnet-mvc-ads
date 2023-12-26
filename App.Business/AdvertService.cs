using App.Data;
using App.Data.Entities;
using App.Models.CategoryDTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Business
{
    public interface IAdvertService
    {
        Task<ServiceResult<ICollection<AdvertEntity>>> GetAdverts(int id);
        Task<ServiceResult<ICollection<AdvertEntity>>> GetLastTenAdverts();
        Task<ServiceResult<List<AdvertEntity>>> GetAllAdvertsUnderGivenCategory(int id);
        Task<ServiceResult<AdvertEntity>> AddAdvertToDb(IFormFile ImageUrl, AdvertEntity advert, int userId);
        

    }


    public class AdvertService : IAdvertService
    {
        private readonly AppDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;
        private readonly ICategoryService _categoryService;
        public AdvertService(AppDbContext dbContext, ILogger<CategoryService> logger , ICategoryService categoryService)
        {
            _categoryService = categoryService;
            _dbContext = dbContext;
        }

        public async Task<ServiceResult<ICollection<AdvertEntity>>> GetAdverts(int id)
        {
            ICollection<AdvertEntity> advertList = await _dbContext.Adverts.Where(x => x.CategoryId == id).ToListAsync();

            return ServiceResult.Success(advertList);
        }

        public async Task<ServiceResult<List<AdvertEntity>>> GetAllAdvertsUnderGivenCategory(int id)
        {
            List<AdvertEntity> allAdverts = new List<AdvertEntity>();
            GetAdvertList(id, allAdverts);

            return ServiceResult.Success(allAdverts);
        }

        public async Task<ServiceResult<ICollection<AdvertEntity>>> GetLastTenAdverts()
        {

            ICollection<AdvertEntity> list = await _dbContext.Adverts
                .Include(a => a.Category) // Include ekleyin
                .OrderByDescending(x => x.CreatedAt)
                .Take(10)
                .ToListAsync();
            return ServiceResult.Success(list);
        }


        private async void GetAdvertList(int categoryId,  List<AdvertEntity> allAdverts)
        {
            // Verilen kategori ID'sine ait ilanları al
            ICollection<AdvertEntity> adverts = GetAdverts(categoryId).Result.Data;
            allAdverts.AddRange(adverts);

            // Kategorinin altındaki tüm alt kategorileri al
            ICollection<CategoryEntity> subCategories =  _categoryService.GetSubCategories(categoryId).Result.Data;
              

            // Her bir alt kategori için recursive olarak ilanları al
            foreach (var subCategory in subCategories)
            {
                GetAdvertList(subCategory.Id, allAdverts);
            }
        }



        private string GetMimeType(string fileName)
        {
            //    var provider = new FileExtensionContentTypeProvider();
            //     if (!provider.TryGetContentType(fileName, out var contentType))
            //      {
            //          contentType = "octet-stream";
            //     }
            return Path.GetExtension(fileName);
            //  return contentType;
        }




        private string GetUploadFolder()
        {
            var uploadsFolder = Path.Combine(Environment.CurrentDirectory, "wwwroot", "images", "adverts");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            return uploadsFolder;
        }


        public async Task<ServiceResult<AdvertEntity>> AddAdvertToDb(IFormFile ImageUrl, AdvertEntity advert , int userId)
        {

            if (ImageUrl != null && ImageUrl.Length > 0)
            {



                var Advert = new AdvertEntity
                {
                    Name = advert.Name,
                    Description = advert.Description,
                    Price = advert.Price,
                    CreatedAt = DateTime.UtcNow,
                    StockCount = advert.StockCount,
                    Confirm = false,
                    CategoryId = advert.CategoryId,
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
                    await fileStream.FlushAsync(); // Stream'i temizle
                }


                using (var stream = new FileStream(fullFilePath, FileMode.Create))
                {
                    await ImageUrl.CopyToAsync(stream);
                }

                _dbContext.Entry(Advert).State = EntityState.Modified;
                await _dbContext.SaveChangesAsync();
            }


            return ServiceResult.Success(advert, StatusCodes.Status201Created);
        }




    }
}

