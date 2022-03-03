using UnifiedProductCatalog.ImportTool.Models;

namespace UnifiedProductCatalog.ImportTool.Persistence
{
    internal interface IProductRepository
    {
        Task SaveImportedProduct(Product product);

        static IProductRepository DefaultRepository { 
            get
            {
                return new EchoProductRepository();
            } 
        }
    }
}
