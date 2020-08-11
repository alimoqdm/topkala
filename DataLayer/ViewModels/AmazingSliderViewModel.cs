using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class AmazingSliderViewModel
    {
        public int ProductID { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int PriceReduction { get; set; }
        public int REduction { get; set; }
        public string ProductImage { get; set; }
        public List<Properties> Propertiess { get; set; }
        public string Name { get; set; }
        public string Parent { get; set; }
        public string Grand { get; set; }
        public bool special { get; set; }
    }
}
