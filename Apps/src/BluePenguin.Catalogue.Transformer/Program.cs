using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Xml.Serialization;

namespace BluePenguin.Catalogue.Transformer
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var inputPath = args[0];
            var outputPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + "\\myFile.xml"; ;
            var metaFiles = Find(args[0]);
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

        }

        private static IEnumerable<string> Find(string path) => Directory.GetFiles(path, "meta.json", SearchOption.AllDirectories);
    }
}