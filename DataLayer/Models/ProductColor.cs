using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
   public class ProductColor
    {
        [Key]
        public int ProductColorId { get; set; }
        [Display(Name ="محصول")]
        public int ProductId { get; set; }
        public Product  product { get; set; }
        [Display(Name ="رنگ محصول")] 
        [Required(ErrorMessage = " لطفا {0} را وارد کنید")]
        [MaxLength(150)]
        public string ColorName { get; set; }
    }
}
