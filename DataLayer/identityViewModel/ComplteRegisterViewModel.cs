using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
    public class ComplteRegisterViewModel
    {
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [Display(Name ="نام")]
        public string Name { get; set; }
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [Display(Name = "نام خانوادگی")]
        public string FamilyName { get; set; }
        
        public int NationalCode { get; set; }

        public bool ForeignNational { get; set; }
        public bool Newsletters { get; set; }
        public int CardNumber { get; set; }
        public string PhoneNumber { get; set; }

    }
}
