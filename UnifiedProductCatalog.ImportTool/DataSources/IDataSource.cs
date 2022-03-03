namespace UnifiedProductCatalog.ImportTool.DataSources
{
    internal interface IDataSource
    {
        Stream GetDataSourceStream(string dataSourcePath);
    }
}
