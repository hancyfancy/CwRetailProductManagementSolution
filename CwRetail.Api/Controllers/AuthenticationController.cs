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

            string userVerificationJson = userVerification.ToJson();

            if (userVerificationJson.IsEmpty())
            {
                return BadRequest("User could not be verified");
            }

            string encryptedUserVerificationJson = userVerificationJson.Encrypt();

            if (encryptedUserVerificationJson.IsEmpty())
            {
                return BadRequest("User verification could not be processed");
            }

            userVerification.EmailVerified = true;
            if (!userVerification.EmailVerified)
            {
                userVerification.Send(UserContactTypeEnum.Email, Settings.SmtpHost, Settings.SmtpPort, Settings.SmtpUseSsl, Settings.SmtpSender, Settings.SmtpPassword, "Verification required", $"Please verify email at https://localhost:7138/api/Authentication/Verify?mode=email&user={encryptedUserVerificationJson}");
            }

            if (!userVerification.PhoneVerified)
            {
                userVerification.SendSms(Settings.TextLocalApiKey, Settings.SmsSender, $"Please verify phone number at https://localhost:7138/api/Authentication/Verify?mode=phone&user={encryptedUserVerificationJson}");
            }

            if (!(userVerification.EmailVerified || userVerification.PhoneVerified))
            {
                return BadRequest("Either email or phone needs to be verified to access content");
            }

            if (userVerification.EmailVerified)
            {
                userVerification.Token = TokenExtensions.GetUniqueKey(UserContactTypeEnum.Email, Settings.EmailValidationSize);

                if (userVerification.Token.IsEmpty())
                {
                    return BadRequest("Error while generating token");
                }

                string validationMessage = $"Please use the following token, which expires in 24 hours, to login: {userVerification.Token}";

                userVerification.Send(UserContactTypeEnum.Email, Settings.SmtpHost, Settings.SmtpPort, Settings.SmtpUseSsl, Settings.SmtpSender, Settings.SmtpPassword, "Validate login attempt", validationMessage);
            }
            else if (userVerification.PhoneVerified)
            {
                userVerification.Token = TokenExtensions.GetUniqueKey(UserContactTypeEnum.Phone, Settings.PhoneValidationSize);

                if (userVerification.Token.IsEmpty())
                {
                    return BadRequest("Error while generating token");
                }

                string validationMessage = $"Please use the following token, which expires in 24 hours, to login: {userVerification.Token}";

                userVerification.SendSms(Settings.TextLocalApiKey, Settings.SmsSender, validationMessage);
            }

            _userTokensRepo.InsertOrUpdate(userVerification.UserId, userVerification.Token);

            return Ok(userVerification.Token);
        }

        [HttpGet(Name = "Verify")]
        public IActionResult Verify(UserContactTypeEnum mode, string user)
        {
            string decryptedUser = user.Decrypt();

            if (decryptedUser.IsEmpty())
            {
                return BadRequest("Invalid user content");
            }

            User retrievedUser = decryptedUser.ToObj<User>();

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

            string retrievedUserJson = retrievedUser.ToJson();

            if (retrievedUserJson.IsEmpty())
            {
                return BadRequest("Retrieved user could not be verified");
            }

            string encryptedRetrievedUserJson = retrievedUserJson.Encrypt();

            if (encryptedRetrievedUserJson.IsEmpty())
            {
                return BadRequest("User could not be processed");
            }

            return Ok(encryptedRetrievedUserJson);
        }
    }
}
