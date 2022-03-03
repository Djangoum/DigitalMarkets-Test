using System.Text.Json;
using UnifiedProductCatalog.ImportTool.DataSources;
using UnifiedProductCatalog.ImportTool.Models;

namespace UnifiedProductCatalog.ImportTool.Factories
{
    internal class SoftwareAdviceDeserializer : IProviderDeserializer
    {
        private const string ExpectedFileExtension = ".json";
        private const string DataSourceName = "Software Advice";
        private readonly IDataSource _dataSource;

        public SoftwareAdviceDeserializer(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public IEnumerable<Product> GetProductsFromSource(string fileLocation)
        {
            if (fileLocation == null)
                throw new ArgumentNullException(nameof(fileLocation));

            if (Path.GetExtension(fileLocation) is string fileExtension && fileExtension != ExpectedFileExtension)
                throw new ArgumentException($"Error found, expected file extension {ExpectedFileExtension} in {DataSourceName} sources found {fileExtension}");

            using var file = _dataSource.GetDataSourceStream(fileLocation);

            var softwareAdviceProducts = JsonSerializer.Deserialize<SoftwareAdviceProductList>(file, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            }) ?? new SoftwareAdviceProductList();

            return softwareAdviceProducts.Products.Select(_ => MapSoftwareAdviceProductToProduct(_));
        }

        private Product MapSoftwareAdviceProductToProduct(SoftwareAdviceProduct softwareAdviceProduct)
            => new Product(softwareAdviceProduct.Title, softwareAdviceProduct.Twitter, softwareAdviceProduct.Categories);
    }
}
