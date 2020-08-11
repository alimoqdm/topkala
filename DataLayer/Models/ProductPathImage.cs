using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DataLayer
{
   public class ProductPathImage
    {
        [Key]
        public int MyProperty { get; set; }
        [Display(Name = "محصول")]
        public int ProductId { get; set; }
        public Product product { get; set; }
        [Display(Name = "تصویر")]
        public string ImagePath { get; set; }
    }
}
