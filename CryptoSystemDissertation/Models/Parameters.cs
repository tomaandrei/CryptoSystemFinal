using System.Xml.Serialization;

namespace CryptoSystemDissertation.Models
{
    public class Parameters
    {
        [XmlAttribute]
        public double Lambda { get; set; }
        [XmlAttribute]
        public double X { get; set; }
        [XmlAttribute]
        public int T { get; set; }
        [XmlAttribute]
        public int A { get; set; }
        [XmlAttribute]
        public int B { get; set; }
        [XmlAttribute]
        public int C0 { get; set; }
                  
    }
}