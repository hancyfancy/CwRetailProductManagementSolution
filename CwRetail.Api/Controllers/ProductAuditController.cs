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

        public ProductAuditController(ILogger<ProductAuditController> logger)
        {
            _logger = logger;
            _repo = new ProductAuditRepository();
        }

        [HttpGet(Name = "GetUpdates")]
        public IActionResult GetUpdates()
        {
            try
            {
                IEnumerable<ProductAudit> productAudits = _repo.GetUpdates();

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
