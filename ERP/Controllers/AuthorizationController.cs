using ERP.Context;
using ERP.DTOs.User;
using ERP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using UserAccount = ERP.Models.UserAccount;

namespace ERP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        public static UserAccount userAccount = new UserAccount();
        private readonly IConfiguration _configuration;

        public AuthorizationController(IConfiguration configuration)
        {
            _configuration = configuration;
            //_userService = userService;
        }


        [HttpPost("register")]
        public async Task<ActionResult<UserAccount>> Register(RegisterUserDTO user)
        {
            CreatePasswordHash(user.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userAccount.Username = user.Username;
            userAccount.PasswordHash = passwordHash;
            userAccount.PasswordSalt = passwordSalt;

            return Ok(userAccount);
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginUserDTO user)
        {
            if (userAccount.Username != user.Username)
            {
                return BadRequest("User not found.");
            }

            if (!VerifyPasswordHash(user.Password, userAccount.PasswordHash, userAccount.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            string token = CreateToken(userAccount);
            return Ok(token);
        }

        private string CreateToken(UserAccount userAccount)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccount.Username),
                new Claim(ClaimTypes.Role, "Admin")
            };

            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
                _configuration.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: creds);

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }


        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }

        }
    }
}
