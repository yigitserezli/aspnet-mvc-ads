using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class BlogCommentsEntity : BaseEntity
    {
        public int UserId { get; set; }

        public int BlogId { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Required, MaxLength(250)]
        public string Content { get; set; }

        public bool Confirm { get; set; }


        // Navigation Properties
        public BlogEntity Blog { get; set; }

        public UserEntity User { get; set; }
    }
}
