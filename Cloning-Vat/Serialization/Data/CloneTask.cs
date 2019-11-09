using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Serialization;

namespace Cloning_Vat.Serialization.Data
{
    /// <summary>
    /// Contains a source and a list of destinations to clone into
    /// </summary>
    [Serializable]
    [XmlType("From")]
    public class CloneTask
    {
        [XmlAttribute(AttributeName = "Path")]
        public string Source { get; set; }

        public CloneDestinationList Destinations { get; set; }

        public override bool Equals(object obj)
        {
            return obj is CloneTask task &&
                   Source == task.Source &&
                   Destinations.SequenceEqual(task.Destinations);
        }

        public override int GetHashCode()
        {
            var hashCode = -1355805956;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Source);
            hashCode = hashCode * -1521134295 + EqualityComparer<CloneDestinationList>.Default.GetHashCode(Destinations);
            return hashCode;
        }
    }
}
