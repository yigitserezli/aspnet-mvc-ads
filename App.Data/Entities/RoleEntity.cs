using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace App.Data.Entities
{
    public class RoleEntity : BaseEntity
    {

        [Required, MaxLength(10)]
        public string Name { get; set; }
        public ICollection<UserEntity> Users { get; set; }

    }
}
