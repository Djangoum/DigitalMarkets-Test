namespace UnifiedProductCatalog.ImportTool.DataSources
{
    internal class FileDataSource : IDataSource
    {
        public Stream GetDataSourceStream(string dataSourcePath)
        {
            return File.OpenRead(dataSourcePath);
        }
    }
}
