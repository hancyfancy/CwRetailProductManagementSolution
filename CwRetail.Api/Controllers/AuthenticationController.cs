﻿using CwRetail.Api.Extensions;
using CwRetail.Data.Enumerations;
using CwRetail.Data.Extensions;
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
        private readonly IUserTokensRepository _userTokensRepo;
        private readonly string _privateRsaKey;

        public AuthenticationController(ILogger<ProductAuditController> logger)
        {
            _logger = logger;
            _userRepo = new UserRepository();
            _userVerificationRepo = new UserVerificationRepository();
            _userRolesRepo = new UserRolesRepository();
            _userTokensRepo = new UserTokensRepository();
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

            _userVerificationRepo.Insert(new UserVerification() { 
                UserId = userId,
                EmailVerified = false,
                PhoneVerified = false
            });

            _userRolesRepo.Insert(userId);

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

            userVerification.EmailVerified = true;
            if (!userVerification.EmailVerified)
            {
                userVerification.SendEmail("Verification required", $"Please verify email at https://localhost:7138/api/Authentication/Verify?mode=email&user={userVerificationJson.Encrypt()}.", null, 0, null);
            }

            if (!userVerification.PhoneVerified)
            {
                userVerification.SendSms("Verification required", $"Please verify phone number at https://localhost:7138/api/Authentication/Verify?mode=phone&user={userVerificationJson.Encrypt()}.");
            }

            if (!(userVerification.EmailVerified || userVerification.PhoneVerified))
            {
                return BadRequest("Either email or phone needs to be verified to access content");
            }

            string token = userVerification.Token = TokenExtensions.GetUniqueKey();

            _userTokensRepo.InsertOrUpdate(userVerification.UserId, token);

            string validationMessage = $"Please use the following token, which expires in 24 hours, to login: {userVerification.Token}";

            userVerification.EmailVerified = false;
            if (userVerification.EmailVerified)
            {
                userVerification.SendEmail("Validate login attempt", validationMessage, null, 0, null);
            }
            else if (userVerification.PhoneVerified)
            {
                userVerification.SendSms("Validate login attempt", validationMessage);
            }

            return Ok(token);
        }

        [HttpGet(Name = "Verify")]
        public IActionResult Verify(UserContactTypeEnum mode, string user)
        {
            User retrievedUser = JsonConvert.DeserializeObject<User>(user.Decrypt());

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

        [HttpPost(Name = "Validate")]
        public IActionResult Validate([FromBody] UserToken uiUserToken)
        {
            User retrievedUser = _userTokensRepo.GetUser(uiUserToken.Token);

            if (retrievedUser is null)
            {
                return BadRequest("Invalid user");
            }

            if (DateTime.UtcNow > retrievedUser.Expiry)
            {
                return BadRequest("Token expired");
            }

            return Ok(_privateRsaKey.CreateToken(retrievedUser));
        }
    }
}
