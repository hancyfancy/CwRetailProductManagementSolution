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
    public class ProductAuditController : ControllerBase
    {
        private readonly ILogger<ProductAuditController> _logger;
        private readonly IProductAuditRepository _repo;
        private readonly string _publicRsaKey;

        public ProductAuditController(ILogger<ProductAuditController> logger)
        {
            _logger = logger;
            _repo = new ProductAuditRepository();
            _publicRsaKey = "";
        }

        [HttpGet(Name = "GetUpdates")]
        public IActionResult GetUpdates([FromHeader] string authorization, [FromHeader] long productId)
        {
            try
            {
                User user =  _publicRsaKey.DecodeToken(authorization.Replace("Bearer", "").Trim());

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
