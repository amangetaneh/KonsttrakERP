using ERP.Context;
using ERP.DTOs.User;
using ERP.Models;
using ERP.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly DataContext context;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public AuthorizationController(DataContext context, IConfiguration configuration, IUserService userService)
        {
            //classes
            this.context = context;

            //interfaces
            _configuration = configuration;
            _userService = userService;
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMe()
        {
            var userName = _userService.GetMyName();
            return Ok(userName);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Employee>> Register(RegisterUserDTO registerDTO)
        {
            
            Employee employee = new();
            employee.FName = registerDTO.FName;
            employee.MName = registerDTO.MName;
            employee.LName = registerDTO.LName;
            employee.Position = registerDTO.Position;
            employee.Status = 0;

            context.Employees.Add(employee);

            await context.SaveChangesAsync();

            UserAccount userAccount = new();

            CreatePasswordHash(registerDTO.Password, out byte[] passwordHash, out byte[] passwordSalt);

            userAccount.Username = registerDTO.Username;
            userAccount.UserRoleId = registerDTO.UserRoleId;
            userAccount.PasswordHash = passwordHash;
            userAccount.PasswordSalt = passwordSalt;
            userAccount.EmployeeId = employee.EmployeeId;

            context.UserAccounts.Add(userAccount);

            await context.SaveChangesAsync();

            return Ok(employee);
        }

        [HttpPost("approve")]
        public async Task<ActionResult<UserAccount>> Approve(ApproveUserDTO approveDTO)
        {
            var userAccount = context.UserAccounts.Where(u => u.Username == approveDTO.Username)
                .Include(u => u.Employee)
                .FirstOrDefault();
            
            if (userAccount == null) return NotFound("User Not Found.");

            userAccount.UserRoleId = approveDTO.UserRoleId;

            var employee = userAccount.Employee;

            employee.Status = 1;

            await context.SaveChangesAsync();

            return Ok(userAccount);
        }
        
        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(LoginUserDTO loginDTO)
        {
            var userAccount = context.UserAccounts.Where(u => u.Username == loginDTO.Username)
                .Include(u => u.Employee)
                .Include(u => u.UserRole)
                .FirstOrDefault();

            if (userAccount == null) return NotFound("User Not Found.");

            if (!VerifyPasswordHash(loginDTO.Password, userAccount.PasswordHash, userAccount.PasswordSalt))
            {
                return BadRequest("Wrong password.");
            }

            var employee = userAccount.Employee;
            var userRole = userAccount.UserRole;

            string token = CreateToken(userAccount, employee);

            return Ok(token);
        }

        private string CreateToken(UserAccount userAccount, Employee employee)
        {

            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userAccount.Username.ToString()),
                new Claim(ClaimTypes.Rsa, userAccount.UserRoleId.ToString()),
                new Claim(ClaimTypes.Sid, employee.EmployeeId.ToString()),
                new Claim(ClaimTypes.Role, AssignRole(employee.Status))
                
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

        private string AssignRole(int status)
        {

            return status == 1 ? "Employee" : "User";
        }
    }
}
