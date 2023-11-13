using System.Xml;
using System.IO;

namespace BluePenguin.Catalogue.AzureFunction
{
    public class IgnoreNamespaceXmlTextReader : XmlTextReader
    {
        public IgnoreNamespaceXmlTextReader(TextReader reader) : base(reader)
        {
        }

        public override string NamespaceURI => "";
    }
}
