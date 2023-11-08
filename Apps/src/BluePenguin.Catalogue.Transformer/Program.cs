using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace BluePenguin.Catalogue.Transformer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputPath = args[0];
            var outputPath = args[1];
            var tempXml = "TempXml";

            var metaFiles = Find(inputPath);
            var root = new Root()
            {
                Products = ExtractProducts(metaFiles).ToList()
            };

            WriteToXml(tempXml, root);
            Transform(tempXml, outputPath);
        }

        private static void Transform(string xmlPath, string outputPath)
        {
            var xsltTransform = new XslTransform();
            xsltTransform.Load(@"Xslt/Transform.xslt");
            xsltTransform.Transform(xmlPath, outputPath);
        }

        private static void WriteToXml(string outputPath, Root root)
        {
            using (StreamWriter strmWriter = new StreamWriter(outputPath,false))
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(Root));
                mySerializer.Serialize(strmWriter, root);
            }
        }

        private static IEnumerable<Product> ExtractProducts(IEnumerable<string> metaFiles)
        {
            foreach (var metaFile in metaFiles)
            {
                using var stream = File.OpenRead(metaFile);
                var productRoot = JsonSerializer.Deserialize<ProductRoot>(stream);
                yield return productRoot.Product;
            }
        }

        private static IEnumerable<string> Find(string path) => Directory.GetFiles(path, "meta.json", SearchOption.AllDirectories);
    }
}