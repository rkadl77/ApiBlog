using APIBlog.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace APIBlog.Models
{
    public class UserRegister
    {
        public string fullName {  get; set; }

        public string password { get; set; }

        [EmailAddress]
        public string email { get; set; }

        public DateTime birthDate { get; set; }

        public GenderEnum gender { get; set; }

        [Phone]
        public string phoneNumber { get; set; }


    }
}
