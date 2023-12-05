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
        public class AdvertCommentsEntity : BaseEntity
        {
    
        
            [Required, MaxLength(250)]
            public string Message { get; set; }
            [Required, Range(1, 5)]
            public byte StarCount { get; set; }
            [DataType(DataType.DateTime)]
            public DateTime CreatedAt { get; set; }
            public bool Confirm { get; set; }



            public int AdvertId { get; set; }
            public AdvertEntity Advert { get; set; }


            public int UserId { get; set; }
     
            public UserEntity User { get; set; }

        }
    }
