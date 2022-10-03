using CwRetail.Api.Enumerations;

namespace CwRetail.Api.Models
{
    public class Product
    {
        public long Id { get; set; }

        //max 100 characters
        public string Name { get; set; }

        //2 decimals
        public decimal Price { get; set; }

        public ProductEnum Type { get; set; }

        public bool Active { get; set; }
    }
}
