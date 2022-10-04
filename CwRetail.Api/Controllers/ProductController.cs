using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

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

        [HttpPost(Name = "Create")]
        public int Create([FromBody] Product product)
        {
            try
            {
                return _context.Products.Insert(product);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductReadController_Get");

                throw;
            }
        }

        [HttpPut(Name = "Edit")]
        public int Edit([FromHeader] long id, [FromBody] JsonElement product)
        {
            try
            {
                string json = product.GetRawText();

                return _context.Products.Update(id, json);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductReadController_Get");

                throw;
            }
        }

        [HttpDelete(Name = "Remove")]
        public int Remove([FromHeader] long id)
        {
            try
            {
                return _context.Products.Delete(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductReadController_Get");

                throw;
            }
        }
    }
}