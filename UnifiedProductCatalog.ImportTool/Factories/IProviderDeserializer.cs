using UnifiedProductCatalog.ImportTool.Models;

namespace UnifiedProductCatalog.ImportTool.Factories
{
    internal interface IProviderDeserializer
    {
        IEnumerable<Product> GetProductsFromSource(string dataSource);
    }
}
