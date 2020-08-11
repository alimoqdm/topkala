using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
   public class ShowCartViewModel
    {
        public string ImageName { get; set; }
        public string Title { get; set; }
        public string Color { get; set; }
        public int Count { get; set; }
        public int Sum { get; set; }
        public int OrderDetailId { get; set; }

    }
}
