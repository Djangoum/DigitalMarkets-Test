using UnifiedProductCatalog.ImportTool.Models;

namespace UnifiedProductCatalog.ImportTool.Persistence
{
    internal class EchoProductRepository : IProductRepository
    {
        public Task SaveImportedProduct(Product product)
        {
            Console.WriteLine($"importing: Name: {product.Name}; Features: {string.Join(',', product.Features)}; Twitter: @{product.Twitter}");
            return Task.CompletedTask;
        }
    }
}
