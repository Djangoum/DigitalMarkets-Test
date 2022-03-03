using UnifiedProductCatalog.ImportTool.DataSources;
using UnifiedProductCatalog.ImportTool.Models;
using YamlDotNet.Serialization;
using YamlDotNet.Serialization.NamingConventions;

namespace UnifiedProductCatalog.ImportTool.Factories
{
    internal class CapterraDeserializer : IProviderDeserializer
    {
        private const string ExpectedFileExtension = ".yaml";
        private const string DataSourceName = "Capterra";
        private readonly static Deserializer _yamlSerializer;
        private readonly IDataSource _dataSource;

        static CapterraDeserializer()
        {
            _yamlSerializer = new DeserializerBuilder()
                .WithNamingConvention(new CamelCaseNamingConvention())
                .Build();
        }

        public CapterraDeserializer(IDataSource dataSource)
        {
            _dataSource = dataSource;
        }

        public IEnumerable<Product> GetProductsFromSource(string fileLocation)
        {
            if (fileLocation == null)
                throw new ArgumentNullException(nameof(fileLocation));

            if (Path.GetExtension(fileLocation) is string fileExtension && fileExtension != ExpectedFileExtension)
                throw new ArgumentException($"Error found, expected file extension {ExpectedFileExtension} in {DataSourceName} sources found {fileExtension}");

            using var fileStream = _dataSource.GetDataSourceStream(fileLocation);

            using var fileReader = new StreamReader(fileStream);

            var capterraModels = _yamlSerializer.Deserialize<CapterraProduct[]>(fileReader);

            return capterraModels.Select(_ => MapCapterraProductToProduct(_));
        }

        private Product MapCapterraProductToProduct(CapterraProduct capterraProduct)
            => new Product(capterraProduct.Name, capterraProduct.Twitter, capterraProduct.Tags.Split(','));
    }
}
