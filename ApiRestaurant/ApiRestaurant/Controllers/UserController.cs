using Microsoft.AspNetCore.Mvc;
using api_restaurant.Data;
using api_restaurant.Models;
using Microsoft.EntityFrameworkCore;
using System.Text;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity.Data;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authorization;

namespace api_restaurant.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly PosContext _context;
        private readonly IConfiguration _configuration;

        public UserController(PosContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user)
        {
            try
            {
                if (!string.IsNullOrEmpty(user.UserName) && !string.IsNullOrEmpty(user.Password))
                {
                    user.Password = EncryptPassword(user.Password);
                }

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok("Create successful!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        private string EncryptPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
            }
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> Users()
        {
            try
            {
                var user = await _context.Users.ToListAsync();
                return Ok(user);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> PutUset(int id, User user)
        {
            try
            {
                // Check if id is valid and matches the user ID
                if (id <= 0 || user.UserId <= 0 || id != user.UserId)
                {
                    return BadRequest("Invalid user ID or ID does not match.");
                }

                // Find the existing user in the database
                var findUser = await _context.Users.FindAsync(id);

                if (findUser == null) // Check if the user exists
                {
                    return NotFound("User not found.");
                }

                findUser.UserName = user.UserName ?? findUser.UserName;

                findUser.Password = !string.IsNullOrWhiteSpace(user.Password)
            ? EncryptPassword(user.Password)
            : findUser.Password;

                findUser.EmployeeId = user.EmployeeId > 0 ? user.EmployeeId : findUser.EmployeeId;

                await _context.SaveChangesAsync();

                return Ok("Update successful!");

                //if (id > 0 && user.UserId > 0 && id == user.UserId)
                //{
                //    var findUser = await _context.Users.FindAsync(id);

                //    if (user != null)
                //    {
                //        user.Password = EncryptPassword(user.Password);
                //        _context.Entry(user).State = EntityState.Modified;
                //        await _context.SaveChangesAsync();
                //        return Ok("Update successful!");
                //    }
                //    return BadRequest("Not found!");
                //}
                //else
                //{
                //    return BadRequest("Bad Request! check id of password");
                //}
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                _context.Users.Remove(user);
                await _context.SaveChangesAsync();

                return Ok("Delete successful!");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] Login loginRequest)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserName == loginRequest.UserName);

            if (user == null || !VerifyPasswordHash(loginRequest.Password, user.Password))
            {
                return Unauthorized("Invalid username or password.");
            }

            var employee = await _context.Employees.FindAsync(user.EmployeeId);

            var claims = new List<Claim>
    {
        //new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Name, "name"),
        //new Claim(ClaimTypes.NameIdentifier, user.EmployeeId.ToString()),
        new Claim(ClaimTypes.NameIdentifier, "1"),
        //new Claim("EmployeeName", employee.EmployeeName)
        new Claim("EmployeeName", "test")
    };

            var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(1), // Extend expiration
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            //return Ok(new
            //{
            //    message = "Login successful",
            //    token = tokenString,
            //    employeeId = user.EmployeeId,
            //    employeeName = employee.EmployeeName
            //});

            return Ok(new
            {
                message = "Login successful",
                token = tokenString,
                employeeId = "1",
                employeeName = "test"
            });
        }


        private bool VerifyPasswordHash(string inputPassword, string storedHash)
        {
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(inputPassword));
            var hash = BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            return hash == storedHash;
        }
    }
}
