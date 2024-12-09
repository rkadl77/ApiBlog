using System.ComponentModel.DataAnnotations;

namespace APIBlog.Models
{
    public class UserLogin
    {
        [EmailAddress]
        public string Email { get; set; }

        public string Password { get; set; }
    }
}

