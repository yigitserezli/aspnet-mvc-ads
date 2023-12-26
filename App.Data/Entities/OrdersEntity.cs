using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    public class OrdersEntity : BaseEntity
    {
        public UserEntity User { get; set; }
        public AdvertEntity Advert { get; set; }

        public int AdvertId { get; set; }
        public int UserId { get; set; }
        public int Quantity { get; set; }
       public decimal Price { get; set; } //yeni eklendi

        [DataType(dataType: DataType.Date)]
        public DateTime OrderDate { get; set; }
        




    }
}
