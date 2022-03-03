using FluentAssertions;
using Moq;
using System;
using System.IO;
using System.Text;
using UnifiedProductCatalog.ImportTool.DataSources;
using UnifiedProductCatalog.ImportTool.Factories;
using Xunit;

namespace UnifiedProductCatalog.ImportTool.Tests
{
    public class CapterraDeserializerShould
    {
        private readonly Mock<IDataSource> _dataSourceMock = new Mock<IDataSource>();
        private CapterraDeserializer? _capterraDeserializer;

        [Theory]
        [InlineData("", true)]
        [InlineData("example.txt", true)]
        [InlineData("example.json", true)]
        [InlineData(null, true)]
        [InlineData("example.yaml", false)]
        public void Throw_An_Exception_When_Non_Yaml_File_Provided(string filePath, bool shouldThrowException)
        {
            _dataSourceMock.Setup(_ => _.GetDataSourceStream(It.IsAny<string>()))
                .Returns(GetStringStreamForYaml());

            _capterraDeserializer = new CapterraDeserializer(_dataSourceMock.Object);

            var deserializationFunc = () => _capterraDeserializer.GetProductsFromSource(filePath);

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

            _capterraDeserializer = new CapterraDeserializer(_dataSourceMock.Object);

            var products = _capterraDeserializer.GetProductsFromSource("capterra.yaml");

            products.Should().HaveCount(3);
            
            foreach(var product in products)
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
---
-
  tags: ""Bugs & Issue Tracking,Development Tools""
  name: ""GitGHub""
  twitter: ""github""
-
  tags: ""Instant Messaging & Chat,Web Collaboration,Productivity""
  name: ""Slack""
  twitter: ""slackhq""
-
  tags: ""Project Management,Project Collaboration,Development Tools""
  name: ""JIRA Software""
  twitter: ""jira""
";

            var bytes = Encoding.UTF8.GetBytes(yamlString);
            return new MemoryStream(bytes);
        }
    }
}
