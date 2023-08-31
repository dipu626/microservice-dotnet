using Catalog.API.Interfaces.Manager;
using Catalog.API.Models;
using CoreApiResponse;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : BaseController
    {
        private readonly IProductManager productManager;

        public CatalogController(IProductManager productManager)
        {
            this.productManager = productManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ResponseCache(Duration = 10)]
        public IActionResult GetProducts()
        {
            try
            {
                IEnumerable<Product> products = this.productManager.GetAll();
                return CustomResult("Get data successfully..", products, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        [ResponseCache(Duration = 10)]
        public IActionResult GetByCategory(string category)
        {
            try
            {
                IEnumerable<Product> products = this.productManager.GetByCategory(category);
                return CustomResult("Get data successfully..", products, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpGet]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult GetById(string id)
        {
            try
            {
                var product = this.productManager.GetById(id);
                return CustomResult("Get data successfully..", product, HttpStatusCode.OK);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPost]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.Created)]
        public IActionResult CreateProduct([FromBody] Product product)
        {
            try
            {
                product.Id = ObjectId.GenerateNewId().ToString();
                bool isSaved = this.productManager.Add(product);

                if (isSaved)
                {
                    return CustomResult("Product has been saved successfully", product, HttpStatusCode.Created);
                }

                return CustomResult("Product saved failed", product, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpPut]
        [ProducesResponseType(typeof(Product), (int)HttpStatusCode.OK)]
        public IActionResult UpdateProduct([FromBody] Product product)
        {
            try
            {
                if (string.IsNullOrEmpty(product.Id))
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                bool isUpdated = this.productManager.Update(product.Id, product);

                if (isUpdated)
                {
                    return CustomResult("Product has been updated successfully", product, HttpStatusCode.OK);
                }

                return CustomResult("Product updated failed", product, HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }

        [HttpDelete]
        //[Route("{id}")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        public IActionResult DeleteProduct(string id)
        {
            try
            {
                if (string.IsNullOrEmpty(id))
                {
                    return CustomResult("Data not found", HttpStatusCode.NotFound);
                }

                bool isDeleted = this.productManager.Delete(id);

                if (isDeleted)
                {
                    return CustomResult("Product has been deleted successfully", HttpStatusCode.OK);
                }

                return CustomResult("Product deleted failed", HttpStatusCode.BadRequest);
            }
            catch (Exception ex)
            {
                return CustomResult(ex.Message, HttpStatusCode.BadRequest);
            }
        }
    }
}
