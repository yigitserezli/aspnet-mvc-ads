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
    public class BlogEntity : BaseEntity
    {

        [Required, MaxLength(50)]
        public string Title { get; set; }

        public int UserId { get; set; }
        [Required, MaxLength(500)]
        public string Content { get; set; }


        [DataType(dataType: DataType.Date)]
        public DateTime CreatedAt { get; set; }


       // public CategoryEntity Category { get; set; }

        public int CategoryId { get; set; }

        //public UserEntity User { get; set; }
        public bool Confirm { get; set; }


        public ICollection<BlogCommentsEntity> BlogComments { get; set; }
    }

    }


