using Cloning_Vat.Serialization.Data;
using System;
using System.IO;
using System.Xml.Serialization;

namespace Cloning_Vat.Serialization
{
    /// <summary>
    /// Translates the <seealso cref="ConfigurationFile"/> to / from XML 
    /// </summary>
    public class ConfigurationSerializer
    {
        /// <summary>
        /// Loads a <seealso cref="ConfigurationFile"/> from XML.  
        /// </summary>
        /// <param name="path">The path to point to the xml file</param>
        /// <returns>a deserialized instance of ConfigurationFile class</returns>
        public ConfigurationFile LoadFromFile(string path)
        {
            if (File.Exists(path) || !path.EndsWith(".xml", StringComparison.OrdinalIgnoreCase))
            {
                using (FileStream stream = new FileStream(path, FileMode.Open))
                {    
                    XmlSerializer deserializer = new XmlSerializer(typeof(ConfigurationFile));
                    return (ConfigurationFile)deserializer.Deserialize(stream);
                    //todo: catch exceptions
                }
            }
            else
            {
                throw new FileNotFoundException($"Unable to find an XML file at '{path}'");
            }
        }

        /// <summary>
        /// Serializes the configuration file class to XML as a string
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
        public string SerializeConfigurationFile(ConfigurationFile configFile)
        {
            using (StringWriter writer = new StringWriter())
            {
                XmlSerializer serializer = new XmlSerializer(typeof(ConfigurationFile));
                serializer.Serialize(writer, configFile);
                // todo: catch exceptions
                return writer.ToString();
            }
        }
    }
}
