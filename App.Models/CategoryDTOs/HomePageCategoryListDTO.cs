using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.CategoryDTOs
{
    public class HomePageCategoryListDTO
    {
        public string categoryName { get; set; }
        public ICollection<SubCategory> subCategories { get; set; }

        public HomePageCategoryListDTO()
        {
            subCategories = new List<SubCategory>();
        }
    }

    public class SubCategory
    {
        public string Name { get; set; }
        public int AdvertCount { get; set; }
    }
}
