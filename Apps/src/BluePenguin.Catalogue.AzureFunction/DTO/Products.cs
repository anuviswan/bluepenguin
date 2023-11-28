using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace BluePenguin.Catalogue.AzureFunction.DTO
{

    [XmlRoot(ElementName = "Product")]
    public class Product
    {

        [XmlElement(ElementName = "Name")]
        public string Name { get; set; }

        [XmlElement(ElementName = "Category")]
        public string Category { get; set; }

        [XmlElement(ElementName = "Collection")]
        public string Collection { get; set; }

        [XmlElement(ElementName = "Code")]
        public string Id { get; set; }

        [XmlElement(ElementName = "Tags")]
        public Tags Tags { get; set; }

        [XmlElement(ElementName = "Size")]
        public string Size { get; set; }

        [XmlElement(ElementName = "MRP")]
        public double MRP { get; set; }

        [XmlElement(ElementName = "DiscountPrice")]
        public double DiscountPrice { get; set; }
    }

    [XmlRoot(ElementName = "Tags")]
    public class Tags
    {
        [XmlElement(ElementName = "Tag")]
        public List<string> Tag { get; set; }
    }

    [XmlRoot(ElementName = "Products")]
    public class Root
    {

        [XmlElement(ElementName = "Product")]
        public List<Product> Products { get; set; }
    }

}
