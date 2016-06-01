using System;
using System.Net;
using System.Web.Http;
using refactor_me.Models;

namespace refactor_me.Controllers
{
    [RoutePrefix("products")]
    public class ProductsController : ApiController
    {
        [Route]
        [HttpGet]
        public Products GetAll()
        {
            return new Products();
        }

        [Route]
        [HttpGet]
        public Products SearchByName(string name)
        {
            return new Products(name);
        }

        [Route("{id}")]
        [HttpGet]
        public Product GetProduct(Guid id)
        {
            var product = new Product(id);

            // format statement for readability
            if (product.IsNew)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return product;
        }

        [Route]
        [HttpPost]
        public void Create(Product product)
        {
            // avoiding null obect reference error
            if (product == null) return;

            product.Save();
        }

        [Route("{id}")]
        [HttpPut]
        public void Update(Guid id, Product product)
        {
            var orig = new Product(id)
            {
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                DeliveryPrice = product.DeliveryPrice
            };

            // format if statement for readability
            if (!orig.IsNew)
            {
                orig.Save();
            }
        }

        [Route("{id}")]
        [HttpDelete]
        public void Delete(Guid id)
        {
            var product = new Product(id);

            // only existing product needs delete 
            if (!product.IsNew)
            {
                product.Delete();
            }
        }

        [Route("{productId}/options")]
        [HttpGet]
        public ProductOptions GetOptions(Guid productId)
        {
            return new ProductOptions(productId);
        }

        [Route("{productId}/options/{id}")]
        [HttpGet]
        public ProductOption GetOption(Guid productId, Guid id)
        {
            var option = new ProductOption(id);

            // throw exception if option is new or option's product is different product
            if (option.IsNew || option.ProductId != productId)
            {
                throw new HttpResponseException(HttpStatusCode.NotFound);
            }

            return option;
        }

        [Route("{productId}/options")]
        [HttpPost]
        public void CreateOption(Guid productId, ProductOption option)
        {
            // avoid null object reference error
            if (option == null) return;
   
            option.ProductId = productId;
            option.Save();
        }

        [Route("{productId}/options/{id}")]
        [HttpPut]
        public void UpdateOption(Guid productId, Guid id, ProductOption option)
        {
            var orig = new ProductOption(id)
            {
                Name = option.Name,
                Description = option.Description
            };

            // format if statement for readability
            // option only saved if it is not new and is the same product
            if (!orig.IsNew && productId == orig.ProductId)
            {
                orig.Save();
            }
        }

        [Route("{productId}/options/{id}")]
        [HttpDelete]
        public void DeleteOption(Guid id)
        {
            var opt = new ProductOption(id);

            // only existing production option needs delete 
            if (!opt.IsNew)
            {
                opt.Delete();
            }
        }
    }
}
