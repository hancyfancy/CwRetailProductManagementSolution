using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace CwRetail.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]/[action]")]
    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly DatabaseContext _context;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            //Need to get valid connection string
            _context = new DatabaseContext(ConnectionStrings.Test);
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<Product> Get()
        {
            try
            {
                //Need to specify limit as a parameter to get
                return _context.Products.Get();
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductReadController_Get");

                throw;
            }
        }
    }
}