using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BluePenguin.Catalogue.Transformer
{
    public class Root
    {
        public List<Product> Products { get; set; }
    }


    // Root myDeserializedClass = JsonConvert.DeserializeObject<Root>(myJsonResponse);

    public class ProductRoot
    {
        public Product Product { get; set; }
    }

    public class Product
    {
        public ProductProfile ProductProfile { get; set; }
        public Specification Specification { get; set; }
        public Cost Cost { get; set; }
    }

    public class ProductProfile
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public List<string> Tags { get; set; }
        public string TimeStamp { get; set; }
    }



    public class Specification
    {
        public double Height { get; set; }
        public double Width { get; set; }
        public double Weight { get; set; }
        public string Shape { get; set; }
    }
    public class Cost
    {
        public double MaterialCost { get; set; }
        public double LabourHours { get; set; }
        public double LabourCost { get; set; }
        public double CreativityCost { get; set; }
        public double PackagingCost { get; set; }
        public double MarketingCost { get; set; }
        public double MRP { get; set; }
        public double DiscountPrice { get; set; }
    }

}
