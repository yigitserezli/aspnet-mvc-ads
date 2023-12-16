using App.Data.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models.CategoryDTOs
{
    public class CategoriesListDTO
    {
        public ICollection<CategoryEntity> CategoriesList { get; set; }
    }
}
