using CwRetail.Api.Helpers;
using CwRetail.Data.Enumerations;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        private readonly IUserRolesRepository _userRolesRepo;
        private readonly string _cryptoKey;
        private readonly string _privateRsaKey;

        public AuthenticationController(ILogger<ProductAuditController> logger)
        {
            _logger = logger;
            _userRepo = new UserRepository();
            _userVerificationRepo = new UserVerificationRepository();
            _userRolesRepo = new UserRolesRepository();
            _cryptoKey = "7kZZdpRXYDFRrPzxrk6HlrGTMq7LTDOQ";
            _privateRsaKey = "";
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

            var userRolesResult = _userRolesRepo.Insert(userId);

            return Ok();
        }

        [HttpPost(Name = "GetUser")]
        public IActionResult GetUser([FromBody] User user)
        {
            UserVerification userVerification = _userVerificationRepo.Get(user.Username);

            if (userVerification is null)
            {
                return BadRequest("User not found");
            }

            userVerification.Username = user.Username;

            string userVerificationJson = JsonConvert.SerializeObject(userVerification);

            if (!userVerification.EmailVerified)
            {
                userVerification.SendEmail("Verification required", $"Please verify email at https://localhost:7138/api/Authentication/Verify?mode=email&user={_cryptoKey.Encrypt(userVerificationJson)}.", null, 0, null);
            }

            if (!userVerification.PhoneVerified)
            {
                userVerification.SendSms("Verification required", $"Please verify phone number at https://localhost:7138/api/Authentication/Verify?mode=phone&user={_cryptoKey.Encrypt(userVerificationJson)}.");
            }

            if (!(userVerification.EmailVerified || userVerification.PhoneVerified))
            {
                return BadRequest("Either email or phone needs to be verified to access content");
            }

            //Need to return JWT token created from requestedUser object
            //The JWT token would then be available to the front end
            //Then pass the JWT token to the controller action methods
            //Controller action methods are responsible for determining if the user retrieved from the JWT token has access to the particular action
            //If the user does not have access to the action, redirect or throw error message accordingly

            //Need to introduce roles for users

            _privateRsaKey.CreateToken(userVerification);

            return Ok();
        }

        [HttpGet(Name = "Verify")]
        public IActionResult Verify(UserContactTypeEnum mode, string user)
        {
            User retrievedUser = JsonConvert.DeserializeObject<User>(_cryptoKey.Decrypt(user));

            if (retrievedUser is null)
            {
                return BadRequest("Invalid user");
            }

            if (mode == UserContactTypeEnum.Email)
            {
                _userVerificationRepo.UpdateEmailVerified(retrievedUser.UserId);
            }
            else if (mode == UserContactTypeEnum.Phone)
            {
                _userVerificationRepo.UpdatePhoneVerified(retrievedUser.UserId);            
            }

            return Ok();
        }
    }
}
