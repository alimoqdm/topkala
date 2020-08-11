using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class AttributeViewModel
    {
        public int ProductID { get; set; }
        public List<int> AttributeID { get; set; }

        public List<string> Val { get; set; }


        public List<string> Att { get; set; }

        public AttributeViewModel()
        {
            Val = new List<string>();
            Att = new List<string>();
            AttributeID = new List<int>();
        }
    }
}
