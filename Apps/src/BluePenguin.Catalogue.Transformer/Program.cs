using System.Text.Json;
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
            var metaFiles = Find(inputPath);
            var productList = new List<Product>();

            foreach (var metaFile in metaFiles)
            {
                using var stream = File.OpenRead(metaFile);
                var productRoot = JsonSerializer.Deserialize<ProductRoot>(stream);
                productList.Add(productRoot.Product);
            }

            var root = new Root()
            {
                Products = productList
            };
            using (StreamWriter strmWriter = new StreamWriter(outputPath))
            {
                XmlSerializer mySerializer = new XmlSerializer(typeof(Root));
                mySerializer.Serialize(strmWriter, root);
            }


            var xsltTransform = new XslTransform();
            xsltTransform.Load(@"Xslt/Transform.xslt");
            xsltTransform.Transform(outputPath, "final.xml");
        }

        private static IEnumerable<string> Find(string path) => Directory.GetFiles(path, "meta.json", SearchOption.AllDirectories);
    }
}