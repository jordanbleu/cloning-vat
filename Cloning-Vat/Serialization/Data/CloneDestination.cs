using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Cloning_Vat.Serialization.Data
{
    [Serializable]
    [XmlType("Into")]
    public class CloneDestination
    {
        public CloneDestination() { }

        [XmlAttribute]
        public string Path { get; set; }

        public CloneDestination(string path)
        {
            Path = path;
        }

        public override string ToString()
        {
            return Path;
        }

        public override bool Equals(object obj)
        {
            return obj is CloneDestination destination && Path.Equals(destination.Path);
        }

        public override int GetHashCode()
        {
            return 467214278 + EqualityComparer<string>.Default.GetHashCode(Path);
        }
    }
}
