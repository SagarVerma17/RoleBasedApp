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
using RoleBasedApp.Models;
using RoleBasedApp.Data;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.EntityFrameworkCore;

namespace RoleBasedLibraryManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : Controller
    {
        public static User user = new User();
        private readonly IConfiguration _configuration;
        private readonly RoleBasedAppContext _dbContext;

        public AuthController(IConfiguration configuration, RoleBasedAppContext DbContext)
        {
            _configuration = configuration;
            _dbContext = DbContext;
        }


        [HttpPost("register")]
        public ActionResult<User> Register(User request)
        {
            if (ModelState.IsValid)
            {
                if (_dbContext.Users.Any(u => u.Username == request.Username))
                {
                    return BadRequest("Username is already taken.");
                }

                string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.PasswordHash);

                User newUser = new User
                {
                    Username = request.Username,
                    PasswordHash = passwordHash,
                    Name = request.Name,
                    Phone = request.Phone,
                };

                _dbContext.Users.Add(newUser);
                _dbContext.SaveChanges();

                return Ok(newUser);
            }

            //return BadRequest(ModelState);
            return View();
        }

        [HttpGet("get-token-data")]
        [Authorize]
        public ActionResult<string> GetTokenData()
        {
            // Retrieve the username from the User Claims
            var username = User?.Identity?.Name;

            return Ok(new { Username = username });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserDTO request)
        {
            User user = _dbContext.Users.SingleOrDefault(u => u.Username == request.Username);

            if (user.Username != request.Username)
            {
                return BadRequest("User not found");
            }

            if (user == null)
            {
                return BadRequest("User not found");
            }

            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash))
            {
                return BadRequest("Wrong Password");
            }

            string token = CreateToken(user);

            var authProperties = new AuthenticationProperties
            {
                ExpiresUtc = DateTime.UtcNow.AddMinutes(30), // Set the expiration time as needed
                IsPersistent = true, // Set to true if you want the cookie to persist across browser sessions
                AllowRefresh = true // Set to true if you want to allow refreshing the token
            };

            await HttpContext.SignInAsync(
                CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Username) }, "custom")),
                authProperties);

            return Ok(user);
        }

        [HttpGet]
        [Authorize]
        public ActionResult<string> GetMyName()
        {
            var userName = User?.Identity?.Name;
            return Ok(new { userName });
        } 

    [HttpGet("get-user-data")]
    public ActionResult<List<Blog>> GetUserData()
    {
        // Retrieve the username from the User Claims
        var username = User?.Identity?.Name;

        // Fetch blog data from the database based on the username
        var blogs = _dbContext.Blog
            .OrderByDescending(b => b.TimeStamp) // Order by TimeStamp in descending order
            .ToList();

        if (blogs == null || !blogs.Any())
        {
            return BadRequest("No blogs found for the user");
        }

        return Ok(blogs);
    }


    private string CreateToken(User user)
        {
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name,user.Username),
                //new Claim(ClaimTypes.Role,"Admin"),
                new Claim(ClaimTypes.Role,"User")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value!));

            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: cred
                );

            var jwt = new JwtSecurityTokenHandler().WriteToken(token);

            return jwt;
        }

        [HttpGet("blogs")]
        [Authorize]
        public IActionResult GetAllBlogsDescending()
        {
            try
            {
                var blogs = _dbContext.Blog
                    .OrderByDescending(blog => blog.TimeStamp)
                    .ToList();

                return Ok(blogs);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("blogs/{title}")]
        public IActionResult CheckTitleExistence(string title)
        {
            var existingBlog = _dbContext.Blog.FirstOrDefault(b => b.Title == title);
            if (existingBlog != null)
            {
                // Title already exists
                return Ok(true);
            }
            else
            {
                // Title does not exist
                return NotFound(false);
            }
        }

        [HttpPut("blogs/{title}")]
        public IActionResult EditBlog(string title, [FromBody] Blog model)
        {
            var blog = _dbContext.Blog.FirstOrDefault(b => b.Title == title);
            if (blog == null)
            {
                return NotFound();
            }

            // Update the non-key properties
            blog.Description = model.Description;
            blog.TimeStamp = DateTime.Now; // Optionally update the modified date

            // Mark the entity as modified and save changes
            _dbContext.Entry(blog).State = EntityState.Modified;
            _dbContext.SaveChanges();

            return Ok();
        }



        [HttpPost("newBlogs")]
        public async Task<IActionResult> AddBlog(Blog blog)
        {
            // Check if the title already exists
            var existingBlog = _dbContext.Blog.FirstOrDefault(b => b.Title == blog.Title);
            if (existingBlog != null)
            {
                // Title already exists, return error response
                return BadRequest("Blog title already exists. Please choose a different title.");
            }

            // Set timestamp for the blog
            blog.TimeStamp = DateTime.Now;

            // Add the blog to the database
            _dbContext.Blog.Add(blog);
            await _dbContext.SaveChangesAsync();

            // Return success response
            return Ok("Blog added successfully.");
        }

        [HttpGet("blog/{title}")]
        public async Task<ActionResult<string>> GetUsernameByTitle(string title)
        {
            try
            {
                // Find the blog with the specified title
                var blog = await _dbContext.Blog.FirstOrDefaultAsync(b => b.Title == title);

                if (blog == null)
                {
                    return NotFound("Blog not found");
                }

                // Return the username associated with the blog
                return Ok(blog.Username);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("blogg/{title}")]
        public async Task<ActionResult<string>> GetUsername(string title)
        {
            try
            {
                // Find the blog with the specified title
                var blog = await _dbContext.Blog.FirstOrDefaultAsync(b => b.Title == title);

                if (blog == null)
                {
                    return NotFound("Blog not found");
                }

                // Return the username associated with the blog
                return Ok(blog);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpDelete("blogs/{title}")]
        public async Task<IActionResult> DeleteBlog(string title)
        {
            try
            {
                var blog = await _dbContext.Blog.FirstOrDefaultAsync(b => b.Title == title);

                if (blog == null)
                {
                    return NotFound("Blog not found");
                }

                _dbContext.Blog.Remove(blog);
                await _dbContext.SaveChangesAsync();

                return Ok("Blog deleted successfully");
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }
    }
}