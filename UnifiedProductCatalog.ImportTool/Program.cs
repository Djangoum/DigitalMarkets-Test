using UnifiedProductCatalog.ImportTool.Factories;
using UnifiedProductCatalog.ImportTool.Persistence;

if (args.Length < 1)
{
    Console.WriteLine("Data provider should be provided");
    return 1;
}

if (args.Length < 2)
{
    Console.WriteLine("Source path should be provided");
    return 2;
}

var provider = args[0];
var fileLocation = args[1];

var deserializerFactory = new ProvidersDeserializersFactory();

var deserializer = deserializerFactory.GetDeserializer(provider);

var products = deserializer.GetProductsFromSource(fileLocation);

var repository = IProductRepository.DefaultRepository;

foreach (var product in products)
    await repository.SaveImportedProduct(product);

return 0;
