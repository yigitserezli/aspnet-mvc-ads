using System.ComponentModel.DataAnnotations;

namespace App.Data.Entities
{
    //TestDenemeComment
    public class UserEntity : BaseEntity
    {
        [Required, Range(3, 50)]
        public string Name { get; set; }

        [Required, Range(3, 50)]
        public string SurName { get; set; }

        [Required]
        public string Address { get; set; }

        [Required, MaxLength(50), EmailAddress]
        public string Username { get; set; } // username -> Email işe eşleşecek.

        [Required, Range(6, 12)]
        public string PasswordHash { get; set; } //Şifreler Hash ile saklanacak.

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }




        public ICollection<AdvertCommentsEntity> AdvertComments { get; set; }
        public ICollection<AdvertEntity> Adverts { get; set; }
        //public ICollection<BlogEntity> Blogs { get; set; }
        //public ICollection<BlogCommentsEntity> BlogComments { get; set; }
        public ICollection<CustomerFavListentity> CustomerFavs { get; set; }
        //public ICollection<UserRoleEntity> UserRoles { get; set; }




    }
}
