using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataLayer
{
    public class ApplicationUser : IdentityUser
    {
        public string Name { get; set; }
        public string FamilyName { get; set; }
        public int NationalCode { get; set; }
        public bool ForeignNational { get; set; }
        public bool Newsletters { get; set; }
        public int CardNumber { get; set; }
    }
}
