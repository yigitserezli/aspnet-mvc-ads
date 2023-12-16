using App.Data;
using App.Data.Entities;
using App.Models;
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
    public interface ICategoryService
    {
        Task<ServiceResult<ICollection<PopularCategoriesDTO>>> GetPopularCategories();
        Task<ServiceResult<ICollection<CategoryEntity>>> GetParentCategories();
        Task<ServiceResult<ICollection<CategoryEntity>>> GetSubCategories(int? id);
        Task<ServiceResult<ICollection<CategoryEntity>>> GetAllCategories();

    }

    public class CategoryService : ICategoryService
    {

        private readonly AppDbContext _dbContext;
        private readonly ILogger<CategoryService> _logger;
        public CategoryService(AppDbContext dbContext,ILogger<CategoryService> logger)
        {

            _dbContext = dbContext;
        }

        public async Task<ServiceResult<ICollection<CategoryEntity>>> GetParentCategories()
        {
            ICollection<CategoryEntity> parentCategoryList = await _dbContext.Categories.Where(x => x.parentCategoryID == null).ToListAsync();

            return ServiceResult.Success(parentCategoryList);


        }
        public async Task<ServiceResult<ICollection<CategoryEntity>>> GetSubCategories(int? id)
        {
            ICollection<CategoryEntity> subCategoryList = await _dbContext.Categories.Where(x => x.parentCategoryID == id).ToListAsync();

            return ServiceResult.Success(subCategoryList);
        }

        public async Task<ServiceResult<ICollection<CategoryEntity>>> GetAllCategories()
        {
            ICollection<CategoryEntity> allCategoriesList = await _dbContext.Categories.ToListAsync();

            return ServiceResult.Success(allCategoriesList);
        }
        public async Task<ServiceResult<ICollection<PopularCategoriesDTO>>> GetPopularCategories()
        {
            var CategoryList = await _dbContext.Categories.Where(x => x.parentCategoryID == null).OrderByDescending(x=>x.categoryPopularityIndex).Take(5).ToListAsync();


           
            ICollection<PopularCategoriesDTO> popularCategoriesDTOList = new List<PopularCategoriesDTO>();

            foreach (var item in CategoryList)
            {
                var popularCategory = new PopularCategoriesDTO();
                popularCategory.PopularCategoryName = item.Name;
                popularCategoriesDTOList.Add(popularCategory);

            }

            return ServiceResult.Success(popularCategoriesDTOList);
        }

       
    }
}
