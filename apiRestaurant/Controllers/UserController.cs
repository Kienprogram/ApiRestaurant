using apiRestaurant.Model;
using apiRestaurant.DTO;
using apiRestaurant.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using apiRestaurant.Utility;

namespace ApiAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly PosContext _context;

        public UserController(PosContext context)
        {
            _context = context;
        }

        [HttpPost("register")]  // Define a route for user registration
        public async Task<IActionResult> Register([FromBody] RegisterDTO request)
        {
            // Validate if the user already exists (for username or email)
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == request.Username || u.Email == request.Email);

            if (existingUser != null)
            {
                return BadRequest("Username or email already taken.");
            }

            // Hash the password
            var hashedPassword = PasswordUtility.HashPassword(request.Password);

            // Create the new user
            var newUser = new User
            {
                Username = request.Username,
                PasswordHash = hashedPassword,  // Store the hashed password
                Email = request.Email
            };

            try
            {
                // Add and save the new user to the database
                _context.Users.Add(newUser);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Log the exception and return a server error response
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            // Return success response
            return Ok("User created successfully.");
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }
    }
}
