using Cloning_Vat.Serialization;
using Cloning_Vat.Serialization.Data;
using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Text;

using System.Linq;
using Cloning_Vat.Helpers;

namespace Tests
{
    public class SerializationTests
    {

        private ConfigurationFile CreateTestConfig()
        {
            return new ConfigurationFile()
            {
                CloneTasks = new List<CloneTask>()
                {
                    new CloneTask()
                    {
                        Source = @"C:\Blah\",
                        Destinations = new CloneDestinationList()
                        {
                            new CloneDestination(@"C:\Taco\Bell\Cheesy\Gordita\Crunch"),
                            new CloneDestination(@"C:\Backup")
                        }
                    },
                    new CloneTask()
                    {
                        Source = @"C:\AnotherFolder",
                        Destinations = new CloneDestinationList()
                        {
                            new CloneDestination(@"C:\Taco\Bell\Double\Chalupa"),
                            new CloneDestination(@"C:\A\B\C")
                        }
                    }
                }
            };
        }

        private string GetExpectedSerializedXml()
        {
            EmbeddedResourceReader resourceReader = new EmbeddedResourceReader();
            return resourceReader.ReadFile("TestXml.Expected.xml");
        }



        [Test]
        public void CanSerialize()
        {
            // Generate a test configuration file form code
            ConfigurationFile file = CreateTestConfig();

            // Serialize that bad boy
            ConfigurationSerializer serializer = new ConfigurationSerializer();
            string serializedConfig = serializer.SerializeConfigurationFile(file);

            // Load the expected xml 
            string expectedSerializedConfig = GetExpectedSerializedXml();

            Assert.AreEqual(serializedConfig, expectedSerializedConfig);
        }

        [Test]
        public void CanDeserialize()
        {
            // Load the expected XML
            string expectedSerializedConfig = GetExpectedSerializedXml();



            // Create a new XML in the current execution directory.  
            // This is weird to do in a unit test but I wanted to mimic 
            // a real life scenario of loading xml straight out of a file
            EasyFileWriter xmlWriter = new EasyFileWriter();
            string xmlFile = xmlWriter.WriteFile(@"TestSandbox\expectedXml.xml", expectedSerializedConfig);

            ConfigurationSerializer serializer = new ConfigurationSerializer();
            ConfigurationFile deserializedConfigurationFile = serializer.LoadFromFile(xmlFile);

            // Assert that all the tasks are equal
            Assert.IsTrue(deserializedConfigurationFile.CloneTasks.SequenceEqual(CreateTestConfig().CloneTasks));
        }     
    }
}