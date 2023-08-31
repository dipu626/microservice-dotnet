using Catalog.API.Interfaces.Manager;
using Catalog.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace Catalog.API.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class CatalogController : ControllerBase
    {
        private readonly IProductManager productManager;

        public CatalogController(IProductManager productManager)
        {
            this.productManager = productManager;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<Product>), (int)HttpStatusCode.OK)]
        public IActionResult GetProducts()
        {
            IEnumerable<Product> products = this.productManager.GetAll();
            return Ok(products);
        }
    }
}
