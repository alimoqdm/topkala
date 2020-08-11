using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class EditProFileViewModel
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public int NationalCode { get; set; }
        public bool ForeignNational { get; set; }
        public bool Newsletters { get; set; }
        public int CardNumber { get; set; }
        public string ID { get; set; }
        public string PhoneNumber { get; set; }
    }
}

