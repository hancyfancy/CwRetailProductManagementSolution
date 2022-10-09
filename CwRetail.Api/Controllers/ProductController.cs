using CwRetail.Data.Enumerations;
using CwRetail.Data.Models;
using CwRetail.Data.Repositories;
using CwRetail.Data.Repositories.Implementation;
using CwRetail.Data.Repositories.Interface;
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
        private readonly IProductRepository _repo;

        public ProductController(ILogger<ProductController> logger, IProductRepository repo)
        {
            _logger = logger;
            _repo = repo;
        }

        [HttpGet(Name = "Get")]
        public IEnumerable<dynamic> Get()
        {
            try
            {
                IEnumerable<Product> products = _repo.Get();

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
                return _repo.Insert(product);
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

                return _repo.Update(id, json);
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
                return _repo.Delete(id);
            }
            catch (Exception e)
            {
                _logger.LogError(e, "ProductController_Remove");

                throw;
            }
        }
    }
}