using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Text;
using UnifiedProductCatalog.ImportTool.DataSources;
using UnifiedProductCatalog.ImportTool.Factories;
using Xunit;

namespace UnifiedProductCatalog.ImportTool.UnitTests
{
    public class SoftwareAdviceDeserializerShould
    {
        private readonly Mock<IDataSource> _dataSourceMock = new Mock<IDataSource>();
        private SoftwareAdviceDeserializer? _softwareAdviceDeserializer;

        [Theory]
        [InlineData("", true)]
        [InlineData("example.txt", true)]
        [InlineData("example.json", false)]
        [InlineData(null, true)]
        [InlineData("example.yaml", true)]
        public void Throw_An_Exception_When_Non_Yaml_File_Provided(string filePath, bool shouldThrowException)
        {
            _dataSourceMock.Setup(_ => _.GetDataSourceStream(It.IsAny<string>()))
                .Returns(GetStringStreamForYaml());

            _softwareAdviceDeserializer = new SoftwareAdviceDeserializer(_dataSourceMock.Object);

            var deserializationFunc = () => _softwareAdviceDeserializer.GetProductsFromSource(filePath);

            if (shouldThrowException)
                deserializationFunc.Should().Throw<ArgumentException>();
            else
                deserializationFunc.Should().NotThrow<ArgumentException>();
        }

        [Fact]
        public void Return_Products_With_All_Properties_Mapped()
        {
            _dataSourceMock.Setup(_ => _.GetDataSourceStream(It.IsAny<string>()))
                .Returns(GetStringStreamForYaml());

            _softwareAdviceDeserializer = new SoftwareAdviceDeserializer(_dataSourceMock.Object);

            var products = _softwareAdviceDeserializer.GetProductsFromSource("softwareadvice.json");

            products.Should().HaveCount(2);

            foreach (var product in products)
            {
                product.Should().NotBeNull();
                product.Twitter.Should().NotBeNullOrEmpty();
                product.Name.Should().NotBeNullOrEmpty();
                product.Features.Should().HaveCountGreaterThan(1);
            }
        }

        private Stream GetStringStreamForYaml()
        {
            var yamlString = @"
{
    ""products"": [
        {
                ""categories"": [
                    ""Customer Service"",
                ""Call Center""
            ],
            ""twitter"": ""@freshdesk"",
            ""title"": ""Freshdesk""
        },
        {
                ""categories"": [
                    ""CRM"",
                ""Sales Management""
            ],
            ""title"": ""Zoho"",
            ""twitter"": ""arielamorgarca1""
        }
    ]
}
";

            var bytes = Encoding.UTF8.GetBytes(yamlString);
            return new MemoryStream(bytes);
        }
    }
}
