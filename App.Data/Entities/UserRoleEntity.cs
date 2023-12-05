using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection.Emit;

namespace App.Data.Entities
{
    public class UserRoleEntity : BaseEntity
    {

        [Required, MaxLength(10)]
        public int UserId { get; set; }
        public int RoleId { get; set; }

        // Navigation Properties
        public UserEntity User { get; set; }
        public RoleEntity Role { get; set; }
    }

}
