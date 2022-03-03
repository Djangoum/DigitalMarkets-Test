namespace UnifiedProductCatalog.ImportTool.Models
{
    internal class Product
    {
        public Product(string name, string twitter, IEnumerable<string> features)
        {  
            Name = name;
            Twitter = twitter;
            Features = features;
        }

        public string Name { get; set; }
        public string Twitter { get; set; }
        public IEnumerable<string> Features { get; set; }
    }
}
