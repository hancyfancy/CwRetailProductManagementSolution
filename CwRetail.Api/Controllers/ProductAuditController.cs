using CwRetail.Api.Extensions;
using CwRetail.Data.Constants;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Asn1.Cms;

namespace CwRetail.Api.Controllers
{
    [System.Web.Http.Cors.EnableCors("*", "*", "*")]
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductAuditController : ControllerBase
    {
        private readonly ILogger<ProductAuditController> _logger;
        private readonly IProductAuditRepository _repo;

        public ProductAuditController(ILogger<ProductAuditController> logger)
        {
            _logger = logger;
            _repo = new ProductAuditRepository();
        }

        [HttpGet(Name = "GetUpdates")]
        public IActionResult GetUpdates([FromHeader] string authorization, [FromHeader] long productId)
        {
            try
            {
                User user = authorization.Replace("Bearer", "").Trim().Decrypt().ToObj<User>();

                if (user is null)
                {
                    return BadRequest("Invalid user");
                }

                if (
                    !(string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardAdmin.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.StandardSpecialist.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.PlatinumUser.ToUpperInvariant())
                    || string.Equals(user.Role.ToUpperInvariant(), UserRoleConstant.GoldUser.ToUpperInvariant()))
                    )
                {
                    return BadRequest("Unauthorised");
                }

                //Check role, if user does not belong to allowed roles, return bad request

                if (productId <= 0)
                {
                    return BadRequest("Invalid product id");
                }

                IEnumerable<ProductAudit> productAudits = _repo.GetUpdates(productId);

                List<dynamic> productAuditDyn = new List<dynamic>();

                for (int i = 0; i < productAudits.Count(); i++)
                {
                    ProductAudit productAudit = productAudits.ElementAt(i);

                    productAuditDyn.Add(new
                    {
                        Id = productAudit.ProductAuditId,
                        Json = productAudit.ObjJson,
                        DateTime = productAudit.AuditDateTime
                    });
                }

                return Ok(productAuditDyn);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductController_Get");

                throw;
            }
        }
    }
}
