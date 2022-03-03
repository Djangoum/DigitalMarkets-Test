using UnifiedProductCatalog.ImportTool.DataSources;

namespace UnifiedProductCatalog.ImportTool.Factories
{
    internal class ProvidersDeserializersFactory
    {
        private readonly Dictionary<string, IProviderDeserializer> _providers = new()
        {
            { "capterra", new CapterraDeserializer(new FileDataSource()) },
            { "softwareadvice", new SoftwareAdviceDeserializer(new FileDataSource()) }
        };

        internal IProviderDeserializer GetDeserializer(string providerName) => _providers[providerName];
    }
}
