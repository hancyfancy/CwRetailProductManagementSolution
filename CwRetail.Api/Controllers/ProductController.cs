using CwRetail.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace CwRetail.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<Product> Get()
        {
            try
            {
                return null;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductReadController_Get");

                throw;
            }
        }
    }
}