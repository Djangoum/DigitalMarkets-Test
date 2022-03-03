namespace UnifiedProductCatalog.ImportTool.Models
{
    internal class SoftwareAdviceProduct
    {
        public string[] Categories { get; set; } = Array.Empty<string>();
        public string Twitter { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
    }

    internal class SoftwareAdviceProductList
    {
        public SoftwareAdviceProduct[] Products { get; set; } = Array.Empty<SoftwareAdviceProduct>();
    }
}
