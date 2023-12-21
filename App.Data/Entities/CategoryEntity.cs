using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class CategoryEntity : BaseEntity
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int? parentCategoryID { get; set; }
        public CategoryEntity? parentCategory { get; set; }

        public ICollection<AdvertEntity> Adverts { get; set; }

        public int categoryPopularityIndex { get; set; } // Bu kategori altındaki ilanlara girildiğinde DB deki bu değer 1 artart böylelikle anasayfadaki popüler kategoriler alanını dinamik olarak değiştirebiliriz. 
    }
}
