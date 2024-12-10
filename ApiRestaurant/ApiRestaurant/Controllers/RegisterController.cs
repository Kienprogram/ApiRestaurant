//using api_restaurant.Models;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Mvc;

//namespace api_restaurant.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class RegisterController : ControllerBase
//    {
//        private readonly UserManager<IdentityUser> _userManager;
//        private readonly IConfiguration _configuration;

//        public RegisterController(UserManager<IdentityUser> userManager, IConfiguration configuration)
//        {
//            _userManager = userManager;
//            _configuration = configuration;
//        }

//        [HttpPost("Register")]
//        public async Task<IActionResult> Register([FromBody] User user)
//        {
//            var users = new IdentityUser { UserName = user.UserName};
//            var result = await _userManager.CreateAsync(users, user.Password);

//            if (result.Succeeded)
//            {
//                await _userManager.AddToRoleAsync(user, user.);
//                return Ok("User registered.");
//            }

//            return BadRequest(result.Errors);
//        }
//    }
//}
