using App.Data.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Models
{
    public class AdvertDto
    {
        [Required, MaxLength(50)]
        public string Name { get; set; }

        public int Id { get; set; }

        [Required, MaxLength(250)]
        public string Description { get; set; }

        [Range(0, double.MaxValue), DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [DataType(DataType.Date)]
        public DateTime CreatedAt { get; set; }

        [DataType(DataType.Date)]
        public DateTime UpdatedAt { get; set; }

        [Range(0, int.MaxValue)]
        public int StockCount { get; set; }

        [Url]
        public string ImageUrl { get; set; }

        public bool? Confirm { get; set; }

        public int CategoryId { get; set; }

        public int UserId { get; set; }
        public ICollection<AdvertCommentsEntity> AdvertComments { get; set; }


        public ICollection<OrderstEntity> Orders { get; set; }
        public ICollection<CustomerFavListentity> CustomerFavList { get; set; }
        //
        public int FavoriteCount
        {
            get { return CustomerFavList?.Count ?? 0; }
        }


    }
}
