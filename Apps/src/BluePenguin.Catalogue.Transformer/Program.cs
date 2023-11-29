using System.Drawing.Imaging;
using System.IO;
using System.Text.Json;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.Xsl;

namespace BluePenguin.Catalogue.Transformer
{
    internal class Program
    {
        private const string TEMP_XML_PATH = "Temp.xml";
        private const string TEMP_THUMBNAIL_PATH = "TempThumbnail";
        /// <summary>
        /// Usage : .exe SourcePath DestinationFileName
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if(!Directory.Exists(TEMP_THUMBNAIL_PATH))
            {
                Directory.CreateDirectory(TEMP_THUMBNAIL_PATH);
            }

            var inputPath = args[0];
            var outputPath = args[1];

            var metaFiles = Find(inputPath);
            var root = new Root()
            {
                Products = ExtractProducts(metaFiles).ToList()
            };

            
            WriteToXml(TEMP_XML_PATH, root);
            Transform(TEMP_XML_PATH, outputPath);
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
                var product = ExtractProductMetaInfo(metaFile);
                yield return product;
            }

            void GenerateThumbnails()
            {

            }

            Product ExtractProductMetaInfo(string metaFilePath)
            {
                using var stream = File.OpenRead(metaFilePath);
                var root = JsonSerializer.Deserialize<ProductRoot>(stream);
                return root.Product;
            }
        }

        private static IEnumerable<string> Find(string path) => Directory.GetFiles(path, "meta.json", SearchOption.AllDirectories);
    }
}