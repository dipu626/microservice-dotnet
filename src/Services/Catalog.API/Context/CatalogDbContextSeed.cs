using Catalog.API.Manager;
using Catalog.API.Models;

namespace Catalog.API.Context
{
    public class CatalogDbContextSeed
    {
        static ProductManager _productManager = new ProductManager();

        public static void Seed()
        {
            Product product = _productManager.GetFirstOrDefault(c => true);

            if (product is null)
            {
                _productManager.Add(GetPreconfiguredProducts());
            }
        }

        private static List<Product> GetPreconfiguredProducts()
        {
            throw new NotImplementedException();
        }
    }
}
