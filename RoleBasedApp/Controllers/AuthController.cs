using BCrypt.Net;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using RoleBasedLibraryManagement.Models;
using System.Security.Claims;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Authorization;

namespace RoleBasedLibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;

        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("register")]
        public ActionResult<User> Register(UserDTO request)
        {
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            user.Username = request.Username;
            user.PasswordHash = passwordHash;

            return Ok(user);
        }

        [HttpPost("login")]
        public ActionResult<User> Login(UserDTO request)
        {
            if (user.Username != request.Username)
            {
                return BadRequest("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(user);

            return Ok(token);
        }

        [HttpGet, Authorize]
        public ActionResult<string> GetMyName()
        {
            var userName = User?.Identity?.Name;
            return Ok(new { userName });
        }

        private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Role,"User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }
    }
}
