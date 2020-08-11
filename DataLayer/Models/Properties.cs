using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
   public class Properties
    {
        [Key]
        public int PropertiesId { get; set; }
         [Display(Name ="محصول")]
        public int ProductId { get; set; }
        public Product  product { get; set; }
        [Display(Name ="ویژگی")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(250)]
        public string Title { get; set; }
        [Display(Name ="مقدار")]
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(400)]
        public string Value { get; set; }

    }
}
