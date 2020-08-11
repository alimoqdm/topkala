using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class ProductViewModel
    {
        public int id { get; set; }
        public string  Name { get; set; }
        public string  EngName { get; set; }
        public int Price { get; set; }
        public int Reduction { get; set; }
        public string Category { get; set; }
        public string Review { get; set; }
        public string Brand { get; set; }
        public string FristImage { get; set; }
        public string color { get; set; }
        public string Comment { get; set; }
        public int CommentCount { get; set; }
        public bool Special { get; set; }
        public int PriceWithReduction { get; set; }

        public List<ProductColor> Colors { get; set; }
        public List<Properties> Propertiess { get; set; }
        public List<ProductPathImage> Images { get; set; }
        public List<ProductAttribute> Attributes { get; set; }



        public ProductViewModel()
        {
            Colors = new List<ProductColor>();
            Propertiess=new List<Properties>();
            Images = new List<ProductPathImage>();
            Attributes = new List<ProductAttribute>();
        }
    }
}
