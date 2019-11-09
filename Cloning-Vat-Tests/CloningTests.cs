using Cloning_Vat.Events;
using Cloning_Vat.Helpers;
using Cloning_Vat.Service;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.IO;

namespace Tests
{
    public class CloningTests
    {
        // This will be populated independently for each test, 
        // since a new instance is created for each test case.
        // The [setup] thing below ensures that the file is re-copied no matter
        // what order the tests are run in
        private string inputFilePath = string.Empty;

        private readonly string expectedFileName = "TextFile.txt";
        private string expectedFileContents = string.Empty;

        private List<string> statuses = new List<string>();

        [SetUp]
        public void PrepareTest()
        {
            if (Directory.Exists("TestSandbox"))
            {
                Directory.Delete("TestSandbox", true);
            }

            // Create a copy of the test xml on local disk
            EmbeddedResourceReader resourceReader = new EmbeddedResourceReader();
            string testXml = resourceReader.ReadFile("TestXml.Test.xml");

            EasyFileWriter fileWriter = new EasyFileWriter();
            inputFilePath = fileWriter.WriteFile(@"TestSandbox\TestInput\input.xml", testXml);

            // Create a copy of the test text file on local disk
            expectedFileContents = resourceReader.ReadFile("TestXml.TextFile.txt");
            fileWriter.WriteFile($@"TestSandbox\TestSource\TextFile.txt", expectedFileContents);
        }

        [Test]
        public void CanCloneDirectories()
        {
            CloningVatService service = new CloningVatService(inputFilePath);
            service.StatusUpdated += Service_StatusUpdated;
            service.ExecuteCloningProcess();

            // Save the statuses into a new text file for fun
            EasyFileWriter easyFileWriter = new EasyFileWriter();
            easyFileWriter.WriteFile(@"Log.txt", string.Join("\n", statuses));

            Assert.IsTrue(File.Exists(@"TestSandbox\TestDestination1\TextFile.txt"));
            Assert.IsTrue(File.Exists(@"TestSandbox\TestDestination2\TextFile.txt"));
                     
        }

        

        private void Service_StatusUpdated(object sender, EventArgs e)
        {
            StatusUpdatedEventArgs args = (StatusUpdatedEventArgs)e;
            statuses.Add(args.Status);
        }
    }
}
