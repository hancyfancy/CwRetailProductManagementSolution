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

            //JWT token can only be created after at least email or phone have been verified
            //If email has not been verified and phone has not been verified then JWT token cannot be generated
            //Send verification email and/or sms for user to complete verification

            //Need to return JWT token created from requestedUser object
            //The JWT token would then be available to the front end
            //Then pass the JWT token to the controller action methods
            //Controller action methods are responsible for determining if the user retrieved from the JWT token has access to the particular action
            //If the user does not have access to the action, redirect or throw error message accordingly
            return Ok();
        }
    }
}
