using CwRetail.Data.Enumerations;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        private readonly DatabaseContext _context;

        public ProductController(ILogger<ProductController> logger)
        {
            _logger = logger;
            //Need to get valid connection string
            _context = new DatabaseContext(ConnectionStrings.Test);
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<dynamic> Get()
        {
            try
            {
                //Need to specify limit as a parameter to get

                IEnumerable<Product> products = _context.Products.Get();

                List<dynamic> productDyn = new List<dynamic>();

                for (int i = 0; i < products.Count(); i++)
                {
                    Product product = products.ElementAt(i);

                    productDyn.Add(new 
                    { 
                        Id = product.Id, 
                        Name = product.Name, 
                        Price = product.Price,
                        Type = product.Type.ToString(),
                        Active = product.Active,
                    });
                }

                return productDyn;
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductController_Get");

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
                _logger.LogError(e, "ProductController_Create");

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
                _logger.LogError(e, "ProductController_Edit");

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
                _logger.LogError(e, "ProductController_Remove");

                throw;
            }
        }
    }
}