using App.Data;
using App.Data.Entities;
using App.Models.CategoryDTOs;
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

            ICollection<AdvertEntity> list = await _dbContext.Adverts.OrderByDescending(x => x.CreatedAt).Take(10).ToListAsync();
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
    }
}

