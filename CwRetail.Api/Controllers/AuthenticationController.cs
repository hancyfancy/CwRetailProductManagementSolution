using CwRetail.Api.Extensions;
using CwRetail.Data.Enumerations;
using CwRetail.Data.Extensions;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using GenCryptography.Data.Models;
using GenCryptography.Data.Repositories.Implementation;
using GenCryptography.Data.Repositories.Interface;
using GenCryptography.Service.Utilities.Implementation;
using GenCryptography.Service.Utilities.Interface;
using GenNotification.Service.Utilities.Implementation;
using GenNotification.Service.Utilities.Interface;
using GenTokenization.Service.Utilities.Implementation;
using GenTokenization.Service.Utilities.Interface;
using GenValidation.Service.Utilities.Implementation;
using GenValidation.Service.Utilities.Interface;
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
        private readonly IKeyGenerator _keyGenerator;
        private readonly IEncryptor _encryptor;
        private readonly IDecryptor _decryptor;
        private readonly IUserEncryptionRepository _userEncryptionRepository;
        private readonly IEmailDespatcher _emailDespatcher;
        private readonly ISmsDespatcher _smsDespatcher;
        private readonly IValidator _emailValidator;
        private readonly IValidator _phoneValidator;
        private readonly ITokenizer _alphanumericTokenizer;
        private readonly ITokenizer _numericTokenizer;

        public AuthenticationController(ILogger<ProductAuditController> logger)
        {
            _logger = logger;
            _userRepo = new UserRepository();
            _userVerificationRepo = new UserVerificationRepository();
            _userRolesRepo = new UserRolesRepository();
            _userTokensRepo = new UserTokensRepository();
            _privateRsaKey = "";
            _keyGenerator = new KeyGenerator();
            _encryptor = new Encryptor();
            _decryptor = new Decryptor();
            _userEncryptionRepository = new UserEncryptionRepository(ConnectionStrings.Test);
            _emailDespatcher = new EmailDespatcher();
            _smsDespatcher = new SmsDespatcher();
            _emailValidator = new EmailValidator();
            _phoneValidator = new PhoneValidator();
            _alphanumericTokenizer = new AlphanumericTokenizer();
            _numericTokenizer = new NumericTokenizer();
        }

        [HttpPost(Name = "CreateUser")]
        public IActionResult CreateUser([FromBody] User user)
        {
            if (!_emailValidator.IsValid(user.Email))
            {
                return BadRequest("Invalid email");
            }

            if (!_phoneValidator.IsValid(user.Phone))
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

            byte[] encryptionKey = _keyGenerator.GenerateEncryptionKey();

            if ((encryptionKey is null) || (encryptionKey.Count() == 0))
            {
                return BadRequest("Failed to generate encryption key");
            }

            _userEncryptionRepository.InsertOrUpdate(userVerification.UserId, encryptionKey);

            string encryptedUserVerificationJson = _encryptor.Encrypt(encryptionKey, userVerificationJson);

            if (encryptedUserVerificationJson.IsEmpty())
            {
                return BadRequest("User verification could not be processed");
            }

            if (!userVerification.EmailVerified)
            {
                _emailDespatcher.SendEmail(Settings.SmtpHost, Settings.SmtpPort, Settings.SmtpUseSsl, userVerification.Email, Settings.SmtpSender, Settings.SmtpPassword, "Verification required", $"Please verify email at https://localhost:7138/api/Authentication/Verify?mode=email&user={encryptedUserVerificationJson}");
            }

            if (!userVerification.PhoneVerified)
            {
                _smsDespatcher.SendSms(Settings.ClickSendUsername, Settings.ClickSendApiKey, userVerification.Phone, Settings.SmsSender, $"Please verify phone number at https://localhost:7138/api/Authentication/Verify?mode=phone&user={encryptedUserVerificationJson}");
            }

            if (!(userVerification.EmailVerified || userVerification.PhoneVerified))
            {
                return BadRequest("Either email or phone needs to be verified to access content");
            }

            if (userVerification.EmailVerified)
            {
                userVerification.Token = _alphanumericTokenizer.GetUniqueKey(Settings.EmailValidationSize);

                if (userVerification.Token.IsEmpty())
                {
                    return BadRequest("Error while generating token");
                }

                string validationMessage = $"Please use the following token, which expires in 24 hours, to login: {userVerification.Token}";

                _emailDespatcher.SendEmail(Settings.SmtpHost, Settings.SmtpPort, Settings.SmtpUseSsl, userVerification.Email, Settings.SmtpSender, Settings.SmtpPassword, "Validate login attempt", validationMessage);
            }
            else if (userVerification.PhoneVerified)
            {
                userVerification.Token = _numericTokenizer.GetUniqueKey(Settings.PhoneValidationSize);

                if (userVerification.Token.IsEmpty())
                {
                    return BadRequest("Error while generating token");
                }

                string validationMessage = $"Please use the following token, which expires in 24 hours, to login: {userVerification.Token}";

                _smsDespatcher.SendSms(Settings.ClickSendUsername, Settings.ClickSendApiKey, userVerification.Phone, Settings.SmsSender, validationMessage);
            }

            _userTokensRepo.InsertOrUpdate(userVerification.UserId, userVerification.Token);

            return Ok(userVerification.Token);
        }

        [HttpGet(Name = "Verify")]
        public IActionResult Verify(UserContactTypeEnum mode, string user)
        {
            string decryptedUser = _decryptor.Decrypt(new byte[] { }, user);

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

            UserEncryption userEncryption = _userEncryptionRepository.Get(retrievedUser.UserId);

            string encryptedRetrievedUserJson = _encryptor.Encrypt(userEncryption.EncryptionKey, retrievedUserJson);

            if (encryptedRetrievedUserJson.IsEmpty())
            {
                return BadRequest("User could not be processed");
            }

            return Ok(encryptedRetrievedUserJson);
        }
    }
}
