using FluentAssertions;
using System;
using System.IO;
using Xunit;

namespace UnifiedProductCatalog.ImportTool.End2EndTests
{
    public class ImportToolShould
    {
        [Fact]
        public void Import_All_Products_For_Capterra_Files()
        {
            using var stringWriter = new StringWriter();

            Console.SetOut(stringWriter);

            var entryPoint = typeof(Program).Assembly.EntryPoint!;
            var result = entryPoint.Invoke(null, new object[] { new string[] { "capterra", "capterra.yaml" } });

            result.Should().Be(0);
            var consoleOutputResult = stringWriter.ToString();
            consoleOutputResult.Should().Be(@"importing: Name: GitGHub; Features: Bugs & Issue Tracking,Development Tools; Twitter: @github
importing: Name: Slack; Features: Instant Messaging & Chat,Web Collaboration,Productivity; Twitter: @slackhq
importing: Name: JIRA Software; Features: Project Management,Project Collaboration,Development Tools; Twitter: @jira
");
        }

        [Fact]
        public void Import_All_Products_For_SoftwareAdvice_Files()
        {
            using var stringWriter = new StringWriter();

            Console.SetOut(stringWriter);

            var entryPoint = typeof(Program).Assembly.EntryPoint!;
            var result = entryPoint.Invoke(null, new object[] { new string[] { "softwareadvice", "softwareadvice.json" } });

            result.Should().Be(0);
            var consoleOutputResult = stringWriter.ToString();
            consoleOutputResult.Should().Be(@"importing: Name: Freshdesk; Features: Customer Service,Call Center; Twitter: @@freshdesk
importing: Name: Zoho; Features: CRM,Sales Management; Twitter: @
");
        }

        [Fact]
        public void Return_Error_Code_2_When_Source_Parameter_Is_Not_Provided()
        {
            using var stringWriter = new StringWriter();

            Console.SetOut(stringWriter);

            var entryPoint = typeof(Program).Assembly.EntryPoint!;
            var result = entryPoint.Invoke(null, new object[] { new string[] { "softwareadvice" } });

            result.Should().Be(2);
            var consoleOutputResult = stringWriter.ToString();
            consoleOutputResult.Should().Be("Source path should be provided\r\n");
        }

        [Fact]
        public void Return_Error_Code_1_When_Provider_Parameter_Is_Not_Provided()
        {
            using var stringWriter = new StringWriter();

            Console.SetOut(stringWriter);

            var entryPoint = typeof(Program).Assembly.EntryPoint!;
            var result = entryPoint.Invoke(null, new object[] { new string[] { } });

            result.Should().Be(1);
            var consoleOutputResult = stringWriter.ToString();
            consoleOutputResult.Should().Be("Data provider should be provided\r\n");
        }
    }
}
