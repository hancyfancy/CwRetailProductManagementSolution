using CwRetail.Api.Extensions;
using CwRetail.Data.Constants;
using CwRetail.Data.Enumerations;
using CwRetail.Data.Extensions;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace CwRetail.Api.Controllers
{
    [System.Web.Http.Cors.EnableCors("*", "*", "*")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductRepository _repo;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            _repo = new ProductRepository();
        }

        [HttpGet(Name = "Get")]
        public IActionResult Get([FromHeader] string authorization)
        {
            try
            {
                string decryptedUser = authorization.Replace("Bearer", "").Trim().Decrypt();

                if (decryptedUser.IsEmpty())
                {
                    return BadRequest("Invalid user content");
                }

                User user = decryptedUser.ToObj<User>();

                if (user is null)
                {
                    return BadRequest("Invalid user");
                }

                if (
                    !(string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardAdmin.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardSpecialist.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.PlatinumUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.GoldUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.SilverUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.BronzeUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardUser.ToUpperInvariant()))
                    )
                {
                    return BadRequest("Unauthorised");
                }

                IEnumerable<Product> products = _repo.Get();

                List<dynamic> productDyn = new List<dynamic>();

                for (int i = 0; i < products.Count(); i++)
                {
                    Product product = products.ElementAt(i);

                    productDyn.Add(new 
                    { 
                        Id = product.ProductId, 
                        Name = product.Name, 
                        Price = product.Price,
                        Type = product.Type.ToString(),
                        Active = product.Active,
                    });
                }

                return Ok(productDyn);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductController_Get");

                throw;
            }
        }

        [HttpPost(Name = "Create")]
        public IActionResult Create([FromHeader] string authorization, [FromBody] Product product)
        {
            try
            {
                string decryptedUser = authorization.Replace("Bearer", "").Trim().Decrypt();

                if (decryptedUser.IsEmpty())
                {
                    return BadRequest("Invalid user content");
                }

                User user = decryptedUser.ToObj<User>();

                if (user is null)
                {
                    return BadRequest("Invalid user");
                }

                if (
                    !(string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardAdmin.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardSpecialist.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.PlatinumUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.GoldUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.SilverUser.ToUpperInvariant()))
                    )
                {
                    return BadRequest("Unauthorised");
                }

                return Ok(_repo.Insert(product));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductController_Create");

                throw;
            }
        }

        [HttpPatch(Name = "Edit")]
        public IActionResult Edit([FromHeader] string authorization, [FromHeader] long id, [FromBody] JsonElement product)
        {
            try
            {
                string decryptedUser = authorization.Replace("Bearer", "").Trim().Decrypt();

                if (decryptedUser.IsEmpty())
                {
                    return BadRequest("Invalid user content");
                }

                User user = decryptedUser.ToObj<User>();

                if (user is null)
                {
                    return BadRequest("Invalid user");
                }

                if (
                    !(string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardAdmin.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardSpecialist.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.PlatinumUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.GoldUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.SilverUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.BronzeUser.ToUpperInvariant()))
                    )
                {
                    return BadRequest("Unauthorised");
                }

                if (id <= 0)
                {
                    return BadRequest("Invalid id");
                }

                string json = product.GetRawText();

                var obj = JsonConvert.DeserializeObject<dynamic>(json);

                foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(obj))
                {
                    var propName = System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(prop.Name.ToLower());

                    var propInfo = typeof(Product).GetProperty(propName);

                    if (propInfo is null)
                    {
                        return BadRequest($"Invalid property name: {propName}");
                    }

                    string expectedType = propInfo.PropertyType.Name;
                    string actualType = prop.GetValue(obj).Value.GetType().Name;

                    if ((!string.Equals(expectedType, "ProductTypeEnum")) && (!string.Equals(actualType, "Double")) && (!string.Equals(actualType, expectedType)))
                    {
                        return BadRequest($"Invalid property type: {propName} should be {expectedType} instead of {actualType}");
                    }
                }

                return Ok(_repo.Update(id, json));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductController_Edit");

                throw;
            }
        }

        [HttpDelete(Name = "Remove")]
        public IActionResult Remove([FromHeader] string authorization, [FromHeader] long id)
        {
            try
            {
                string decryptedUser = authorization.Replace("Bearer", "").Trim().Decrypt();

                if (decryptedUser.IsEmpty())
                {
                    return BadRequest("Invalid user content");
                }

                User user = decryptedUser.ToObj<User>();

                if (user is null)
                {
                    return BadRequest("Invalid user");
                }

                if (
                    !(string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardAdmin.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardSpecialist.ToUpperInvariant()))
                    )
                {
                    return BadRequest("Unauthorised");
                }

                if (id <= 0)
                {
                    return BadRequest("Invalid id");
                }

                return Ok(_repo.Delete(id));
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductController_Remove");

                throw;
            }
        }
    }
}