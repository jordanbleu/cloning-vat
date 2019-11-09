using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Serialization;

namespace Cloning_Vat.Serialization.Data
{
    /// <summary>
    /// This is the wrapper for the entire xml configuration, containing info about all our 
    /// cloning tasks
    /// </summary>
    [Serializable]
    [XmlRoot(ElementName="Instructions")]
    public class ConfigurationFile
    {
        /// <summary>
        /// Set of task for the full cloning operation
        /// </summary>
        public List<CloneTask> CloneTasks { get; set; }

    }
}
