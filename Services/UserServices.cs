using APIBlog.Data;
using APIBlog.Models;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Linq;

namespace APIBlog.Services
{
    public interface IUserService
    {
        void RegisterUser(UserRegister model);
        User GetUserByEmail(string email);
        string GenerateJwtToken(User user);
    }

    public class UserService : IUserService
    {
        private readonly IPasswordHasher _passwordHasher;
        private readonly string _jwtSecret;
        private readonly ApplicationDbContext _dbContext;

        public UserService(IPasswordHasher passwordHasher, IConfiguration configuration, ApplicationDbContext dbContext)
        {
            _passwordHasher = passwordHasher;
            _jwtSecret = configuration["Jwt:Secret"];
            _dbContext = dbContext;
        }

        public void RegisterUser(UserRegister model)
        {
            string hashedPassword = _passwordHasher.HashPassword(model.password);

            var user = new User
            {
                FullName = model.fullName,
                PasswordHash = hashedPassword,
                Email = model.email,
                BirthDate = model.birthDate,
                Gender = model.gender.ToString(),
                PhoneNumber = model.phoneNumber
            };

            _dbContext.Users.Add(user);
            _dbContext.SaveChanges();
        }

        public User GetUserByEmail(string email)
        {
            return _dbContext.Users.FirstOrDefault(u => u.Email == email);
        }

        public string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSecret);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Email)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }
}