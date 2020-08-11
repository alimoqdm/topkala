using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class ShowOrderViewModel
    {
        public int OrderDetailId { get; set; }
        public string ImageName { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public int Price { get; set; }
        public int Sum { get; set; }
       
    }
}
