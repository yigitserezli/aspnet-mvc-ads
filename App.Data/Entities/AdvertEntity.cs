using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class AdvertEntity : BaseEntity
    {

        [Required, MaxLength(50)]
        public string Name { get; set; }


        [Required, MaxLength(250)]
        public string Description { get; set; }

        [Range(0, double.MaxValue), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(dataType: DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(dataType: DataType.Date)]
        public DateTime UpdatedAt { get; set; }

        [Range(0, int.MaxValue)]
        public int StockCount { get; set; }


        [Url]
        public string ImageUrl { get; set; } //ekleyelim mi ?

        public bool Confirm { get; set; }


        // Navigation Properties
       
        public int CategoryId { get; set; }
        public CategoryEntity Category { get; set; }




        public UserEntity User { get; set; }
        public int UserId { get; set; }


        public ICollection<AdvertCommentsEntity> AdvertComments { get; set; }
        
       



    }
}
