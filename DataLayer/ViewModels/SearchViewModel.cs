using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class SearchViewModel
    {
        public int ProductID { get; set; }
        public int CategoryId { get; set; }
        public string ProductName { get; set; }
        public int ProductPrice { get; set; }
        public int PriceReduction { get; set; }
        public bool special { get; set; }
        public int Visit { get; set; }
        public string ProductImage { get; set; }

        public DateTime Date { get; set; }
        public int count { get; set; }
        public List<Category> Categoryies { get; set; }
        public string CategoryName { get; set; }
        public string CategoryParentName { get; set; }
        public string CategoryGrandParentName { get; set; }
    }
}
