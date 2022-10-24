using CwRetail.Api.Helpers;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;

namespace CwRetail.Api.Controllers
{
    [System.Web.Http.Cors.EnableCors("*", "*", "*")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly ILogger<ProductAuditController> _logger;
        private readonly IUserRepository _userRepo;
        private readonly IUserVerificationRepository _userVerificationRepo;

        public AuthenticationController(ILogger<ProductAuditController> logger)
        {
            _logger = logger;
            _userRepo = new UserRepository();
            _userVerificationRepo = new UserVerificationRepository();
        }

        [HttpPost(Name = "CreateUser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!user.Email.IsValidEmail())
            {
                return BadRequest("Invalid email");
            }

            if (!user.Phone.IsValidPhone())
            {
                return BadRequest("Invalid phone");
            }

            long userId = _userRepo.Insert(user);

            var userVerificationResult = _userVerificationRepo.Insert(new UserVerification() { 
                UserId = userId,
                EmailVerified = false,
                PhoneVerified = false
            });

            return Ok(userVerificationResult);
        }

        [HttpPost(Name = "GetUser")]
        public IActionResult GetUser([FromBody] User user)
        {
            User requestedUser = _userRepo.Get(user.Username);

            if (requestedUser is null)
            {
                return BadRequest("User not found");
            }

            requestedUser.Username = user.Username;

            return Ok(userVerificationResult);
        }
    }
}
